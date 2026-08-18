# HooksHangMore

Allows you to hang more items on lamp hooks. 

### Items enabled to hang

* Fishing Rod
* Broom
* Chip Log
* Quadrant
* Knife
* Fish
* Bucket
* Kettle
* Hammer
* Oar
* Pot
* Big Pot
* Metal Mug
* Wooden Mug
* Anchor
* Handheld Anemometer (from [Windicators](https://github.com/NANDbrew/Windicators))

#### Fishing Rod

If you cast the line first and then attach the rod, fishing will continue. The rate at which you catch fish will be lower than if were holding the fishing rod. The rate while in the fishing rod holder will be higher when you are out to sea.  
<br>
The idle fishing mod is not needed, however this mod is compatible with it. The catch rate when in the holder is higher than if the rod is loose on the deck.  

#### Chip Log

If you throw the log line in the water first and then attach the chip log, speed measurement will continue.  

#### Fish

Fish will dry while hung on a lamp hook. They will not dry out completely before rotting if unsalted. You can salt a fish before or after hanging it.  
<br>
This is compatible with [CookedInfo](https://github.com/alesparise/CookedInfo-Sailwind-Mod). If you have it installed you will see the drying status.  

#### Pots, Mugs, and Bucket

These items must be emptied before you hang them.  

#### Anchor

While the anchor is attached, you can adjust the length of the rope up to a certain point. You cannot move the vessel by tightening the rope while the anchor is attached to a hook that is not on the ship. Ships added by a mod might not keep the anchor attached when loading a save or returning to it after being far away from it.  

#### Lamp Hooks

While holding the key for rotating held item (default Q) and scrolling the mouse wheel, you can rotate the lamp hook clockwise or counter-clockwise up to 90°. This allows you to attach it to a surface in more ways than just the default up, which then gives you more options for attaching items to it.  

## For Other Mod Authors

If you wish to use this to be able to attach a custom item you made to a hook:  
1. Add this mod as a soft BepInEx dependency.
2. Make sure your item has a ShipItem component or has a component which inherits from ShipItem.
3. In your main plugin class, before your assets are loaded, call the exposed function to add offsets for your item. You can use either the prefab name or name that is in ShipItem. The parameters are string name, Vector3 position_offset, Vector3 rotation_offset.  

Example:
```c#
foreach (var plugin in Chainloader.PluginInfos)
{
    var metadata = plugin.Value.Metadata;
    if (metadata.GUID.Equals(HOOKS_HANG_MORE_GUID))
    {
        LogInfo("Hooks Hang More mod found");
        var hhm = Traverse.Create(plugin.Value.Instance);

        hhm.Method("AddAttachedOffset", "custom item(Clone)", new Vector3(0f, 0.2f, -0.13f), new Vector3(270f, 0f, 0f)).GetValue<bool>();
    }
}
```

### Requires

* [BepInEx 5.4.23](https://github.com/BepInEx/BepInEx/releases)

### Installation

If updating, remove HooksHangMore folders and/or HooksHangMore.dll files from previous installations.  
<br>
Extract the downloaded zip. Inside the extracted HooksHangMore-\<version\> folder copy the HooksHangMore folder and paste it into the Sailwind/BepInEx/Plugins folder.  

#### Consider supporting me 🤗

<a href='https://www.paypal.com/donate/?business=WKY25BB3TSH6E&no_recurring=0&item_name=Thank+you+for+your+support%21+I%27m+glad+you+are+enjoying+my+mods%21&currency_code=USD' target='_blank'><img src="https://www.paypalobjects.com/en_US/i/btn/btn_donate_LG.gif" border="0" alt="Donate with PayPal button" />
<a href='https://ko-fi.com/S6S11DDLMC' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://storage.ko-fi.com/cdn/kofi6.png?v=6' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>