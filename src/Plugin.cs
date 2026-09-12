using BepInEx;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DSPSphereBuilder
{
    [BepInPlugin("dsp.spherebuilder", "DSP Sphere Builder", BuildInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private PaintSession session;
        private UIDysonEditor attachedEditor;
        private DysonSphereLayer displayedLayer;
        private RectTransform panel;
        private Button paintButton;
        private Text status;
        private readonly Vector3[] toolbarCorners = new Vector3[4];
        private float refreshAt;

        private void Awake()
        {
            var target = typeof(GameMain).Assembly.ManifestModule.ModuleVersionId.ToString();
            session = new PaintSession(target, message => Logger.LogError(message));
            Logger.LogInfo("DSP Sphere Builder " + BuildInfo.Label + "; target MVID " + target);
        }

        private void Update()
        {
            try
            {
                var editor = NativeTarget.Editor;
                if (!ReferenceEquals(editor, attachedEditor)) Detach();
                if (editor == null || !editor.active)
                {
                    if (panel != null) panel.gameObject.SetActive(false);
                    return;
                }
                if (panel == null)
                {
                    if (session.Stopped) return;
                    Detach();
                    Attach(editor);
                }
                panel.gameObject.SetActive(true);
                PositionPanel(editor);
                var selected = editor.selection?.singleSelectedLayer;
                if (!ReferenceEquals(selected, displayedLayer) || Time.unscaledTime >= refreshAt)
                {
                    displayedLayer = selected;
                    Present(session.Describe(NativeTarget.Resolve));
                    refreshAt = Time.unscaledTime + 0.25f;
                }
            }
            catch (Exception error)
            {
                session.Stop(error);
                Detach();
            }
        }

        private void Attach(UIDysonEditor editor)
        {
            var font = editor.GetComponentsInChildren<Text>(true).First(t => t.font != null).font;
            var root = new GameObject("SphereBuilder", typeof(RectTransform), typeof(Image));
            panel = (RectTransform)root.transform;
            attachedEditor = editor;
            panel.SetParent(editor.controlPanel.transform, false);
            panel.anchorMin = panel.anchorMax = new Vector2(0.5f, 0.5f);
            panel.pivot = new Vector2(1, 0);
            panel.sizeDelta = new Vector2(280, 64);
            root.GetComponent<Image>().color = new Color(0.06f, 0.09f, 0.13f, 0.97f);
            root.AddComponent<UIBlockZone>();
            editor.guiRects = editor.guiRects.Concat(new[] { panel }).ToArray();
            Label(panel, "Sphere Builder", 8, 8, 104, 24, 14, font);

            var button = new GameObject("Paint", typeof(RectTransform), typeof(Image), typeof(Button));
            Place(button, panel, 120, 6, 152, 28);
            var background = button.GetComponent<Image>();
            background.color = new Color(0.13f, 0.28f, 0.36f, 1);
            paintButton = button.GetComponent<Button>();
            paintButton.targetGraphic = background;
            paintButton.onClick.AddListener(Paint);
            var caption = Label(button.transform, "Paint next patch", 0, 0, 152, 28, 14, font);
            caption.alignment = TextAnchor.MiddleCenter;
            status = Label(panel, "", 8, 40, 264, 18, 13, font);
            panel.SetAsLastSibling();
            refreshAt = 0;
        }

        private static void Place(GameObject item, Transform parent, float x, float y, float width, float height)
        {
            var rect = (RectTransform)item.transform;
            rect.SetParent(parent, false);
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(x, -y);
            rect.sizeDelta = new Vector2(width, height);
        }

        private static Text Label(Transform parent, string caption, float x, float y, float width, float height, int size, Font font)
        {
            var item = new GameObject("Label", typeof(RectTransform), typeof(Text));
            Place(item, parent, x, y, width, height);
            var text = item.GetComponent<Text>();
            text.font = font; text.fontSize = size; text.color = new Color(1, 1, 1, 1);
            text.text = caption; text.raycastTarget = false;
            return text;
        }

        private void Paint()
        {
            try
            {
                var result = session.Paint(NativeTarget.Resolve);
                Present(result);
                if (result.State == PaintState.Applied || result.State == PaintState.Complete)
                    Logger.LogInfo($"Star {result.StarId}, layer {result.LayerId}, radius {result.Radius}: {result.CompletedPatches}/12 patches; {result.NodeCount} nodes, {result.FrameCount} frames.");
                refreshAt = Time.unscaledTime + 0.25f;
            }
            catch (Exception error)
            {
                session.Stop(error);
                Detach();
            }
        }

        private void Present(PaintResult result)
        {
            status.text = Feedback.Text(result);
            var textHeight = Math.Max(18, status.preferredHeight);
            ((RectTransform)status.transform).sizeDelta = new Vector2(264, textHeight);
            panel.sizeDelta = new Vector2(280, 46 + textHeight);
            paintButton.interactable = result.CanPaint;
        }

        private void PositionPanel(UIDysonEditor editor)
        {
            editor.controlPanel.toolbox.selfRect.GetWorldCorners(toolbarCorners);
            var corner = editor.controlPanel.transform.InverseTransformPoint(toolbarCorners[0]);
            panel.localPosition = new Vector3(corner.x - 12, corner.y, 0);
        }

        private void Detach()
        {
            var oldPanel = panel;
            var oldButton = paintButton;
            var oldEditor = attachedEditor;
            panel = null; paintButton = null; status = null; attachedEditor = null; displayedLayer = null;
            if (oldButton != null) oldButton.onClick.RemoveListener(Paint);
            if (oldEditor != null && oldEditor.guiRects != null && !ReferenceEquals(oldPanel, null))
                oldEditor.guiRects = oldEditor.guiRects.Where(r => !ReferenceEquals(r, oldPanel)).ToArray();
            if (oldPanel != null) Destroy(oldPanel.gameObject);
        }

        private void OnDestroy() => Detach();
    }
}
