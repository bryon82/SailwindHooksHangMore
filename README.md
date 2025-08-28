# HooksHangMore

Allows you to hang more items on lamp hooks. 

### Items enabled to hang

* Fishing Rod
* Broom
* Chip Log
* Quadrant
* Knives
* Fish

#### Fishing Rod

If you cast the line first and then attach the rod, fishing will continue. The rate at which you catch fish will be lower than if were holding the fishing rod. The rate while in the fishing rod holder will be higher when you are out to sea.  
<br>
The idle fishing mod is not needed, however this mod is compatible with it. The catch rate when in the holder is higher than if the rod is loose on the deck.  

#### Chip Log

If you throw the log line in the water first and then attach the chip log, speed measurement will continue.  

### Fish

Fish will dry while hung on a lamp hook. They will not dry out completely if unsalted. You can salt a fish before or after hanging it.  
<br>
This is compatible with [CookedInfo](https://github.com/alesparise/CookedInfo-Sailwind-Mod). If you have it installed you will see the drying status.  

## For Other Mod Authors

If you wish to use this to be able to hang a custom item you made:  
1. Add this mod dll as a reference.
2. Add this mod as a BepInEx dependency.
3. Have your item inherit from ShipItem.
4. Override OnLoad, in it add the HolderAttachable component and set the offsets so your item hangs properly. PositionOffset and RotationOffset are both of type Vector3. If you do not set them then they default to Vector3.zero.
5. Override AllowOnItemClick to check if you are clicking on a holder and that the holder is not occupied.  

Example:
```c#
public override void OnLoad()
{
    initialHoldDistance = holdDistance;
    var attachable = gameObject.AddComponent<HolderAttachable>();
    attachable.PositionOffset = new Vector3(0.02f, -0.15f, -0.12f);
    attachable.RotationOffset = new Vector3(270f, 270f, 0f);
}

public override bool AllowOnItemClick(GoPointerButton lookedAtButton)
{
    if (lookedAtButton.GetComponent<ShipItemHolder>() != null && !lookedAtButton.GetComponent<ShipItemHolder>().IsOccupied)
        return true;

    return false;
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