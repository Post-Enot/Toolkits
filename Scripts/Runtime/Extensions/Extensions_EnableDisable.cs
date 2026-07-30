using UnityEngine;
using UnityEngine.UIElements;

namespace PostEnot.Toolkits
{
    public static class Extensions_EnableDisable
    {
        #region Enable
        public static void Enable(this Behaviour self) => self.enabled = true;

        public static bool Enable(this Collider self) => self.enabled = true;

        public static void Enable(this VisualElement self) => self.SetEnabled(true);

        public static void Activate(this GameObject self) => self.SetActive(true);
        #endregion

        #region Disable
        public static void Disable(this Behaviour self) => self.enabled = false;

        public static bool Disable(this Collider self) => self.enabled = false;

        public static void Disable(this VisualElement self) => self.SetEnabled(false);

        public static void Deactivate(this GameObject self) => self.SetActive(false);
        #endregion

        #region SetEnabled
        public static void SetEnabled(this Behaviour self, bool isEnabled) => self.enabled = isEnabled;

        public static void SetEnabled(this Collider self, bool isEnabled) => self.enabled = isEnabled;
        #endregion

        #region ToggleEnable
        public static void ToggleEnable(this Behaviour self) => self.enabled = !self.enabled;

        public static void ToggleEnable(this Collider self) => self.enabled = !self.enabled;

        public static void ToggleEnable(this VisualElement self) => self.SetEnabled(!self.enabledSelf);

        public static void ToggleActive(this GameObject self) => self.SetActive(!self.activeSelf);
        #endregion
    }
}
