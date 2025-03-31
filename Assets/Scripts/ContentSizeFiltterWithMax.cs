using System;

namespace UnityEngine.UI
{
    [AddComponentMenu("Layout/Content Size Fitter With Max", 141)]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class ContentSizeFitterWithMax : ContentSizeFitter
    {
        [NonSerialized]
        private RectTransform m_Rect;

        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                {
                    m_Rect = GetComponent<RectTransform>();
                }

                return m_Rect;
            }
        }

        [SerializeField]
        private float m_MaxWidth = -1;

        public float maxWidth
        {
            get => m_MaxWidth;
            set => m_MaxWidth = value;
        }

        [SerializeField]
        private float m_MaxHeight = -1;

        public float maxHeight
        {
            get => m_MaxHeight;
            set => m_MaxHeight = value;
        }

        public override void SetLayoutHorizontal()
        {
            base.SetLayoutHorizontal();

            if (maxWidth > 0)
            {
                if (horizontalFit == FitMode.MinSize)
                {
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Min(LayoutUtility.GetMinSize(m_Rect, 0), maxWidth));
                }
                else if (horizontalFit == FitMode.PreferredSize)
                {
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Min(LayoutUtility.GetPreferredSize(m_Rect, 0), maxWidth));
                }
            }
        }

        public override void SetLayoutVertical()
        {
            base.SetLayoutVertical();

            if (maxHeight > 0)
            {
                if (verticalFit == FitMode.MinSize)
                {
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Min(LayoutUtility.GetMinSize(m_Rect, 1), maxHeight));
                }
                else if (verticalFit == FitMode.PreferredSize)
                {
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Min(LayoutUtility.GetPreferredSize(m_Rect, 1), maxHeight));
                }
            }
        }
    }
}