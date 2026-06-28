using HarmonyLib;
using UnityEngine;

namespace HooksHangMore.Patches
{
    internal class UIPatches
    {
        [HarmonyPatch(typeof(LookUI), "ShowLookText")]
        private class LookUIPatches
        {
            public static void Postfix(
                GoPointerButton button,
                TextMesh ___controlsText,
                GoPointer ___pointer,
                TextMesh ___textLicon,
                TextMesh ___textRIcon,
                ref bool ___showingIcon)
            {
                var lampHook = button == null ? null : button.GetComponent<ShipItemLampHook>();
                var holder = lampHook == null ? null : lampHook.GetComponent<AttachableItemHolder>();
                var heldItem = ___pointer == null ? null : ___pointer.GetHeldItem();
                var attachableItem = heldItem == null ? null : heldItem.GetComponent<AttachableItem>();
                var hangableItem = heldItem == null ? null : heldItem.GetComponent<HangableItem>();
                if (lampHook != null && heldItem != null && holder != null && holder.IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(false);
                    ___showingIcon = false;
                    ___controlsText.text = "";

                    if ((bool)heldItem.GetComponent<ShipItemHammer>() && ShipItemHammer.CanNail(button.GetComponent<ShipItem>()))
                    {
                        ___showingIcon = true;
                        if (button.GetComponent<ShipItem>().nailed)
                        {
                            ___textRIcon.gameObject.SetActive(true);
                            ___controlsText.text = "\nunlock";
                        }
                        else
                        {
                            ___textRIcon.gameObject.SetActive(true);
                            ___controlsText.text = "\nlock";
                        }
                    }
                }
                else if (lampHook != null && heldItem != null && (attachableItem != null || hangableItem != null) && !holder.IsOccupied)
                {
                    var shipItem = heldItem.GetComponent<ShipItem>();
                    var itemName = shipItem?.name ?? "anchor";
                    ___textLicon.gameObject.SetActive(true);
                    ___showingIcon = true;
                    ___controlsText.text = $"attach {itemName}\n";

                    if ((bool)heldItem.GetComponent<ShipItemHammer>() && ShipItemHammer.CanNail(button.GetComponent<ShipItem>()))
                    {
                        if (button.GetComponent<ShipItem>().nailed)
                        {
                            ___textRIcon.gameObject.SetActive(true);
                            ___controlsText.text = $"attach {itemName}\nunlock";
                        }
                        else
                        {
                            ___textRIcon.gameObject.SetActive(true);
                            ___controlsText.text = $"attach {itemName}\nlock";
                        }
                    }
                }
            }
        }
    }
}
