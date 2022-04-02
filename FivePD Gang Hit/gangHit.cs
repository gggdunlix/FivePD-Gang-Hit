using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using FivePD.API;
using FivePD.API.Utils;
using CitizenFX.Core.Native;



[CalloutProperties("Gang Hit", "GGGDunlix", "0.1.0")]
public class HitAndRunFatal : FivePD.API.Callout
{
    private Ped suspect1, suspect2, suspect3, suspect4, driver1, passenger1;
    private Vehicle van1;

    public HitAndRunFatal()
    {

        InitInfo(Game.PlayerPed.Position);

        ShortName = "Gang Hit";
        CalloutDescription = "A Gang has authorized a hit on you, and members are on the way to attack you. Code 99 Backup requested.";
        ResponseCode = 3;
        StartDistance = 150f;
    }

    public override async Task OnAccept()
    {
        InitBlip(150f, BlipColor.Red);
       
        PlayerData playerData = Utilities.GetPlayerData();
        string CallSign = playerData.Callsign;
        ShowNetworkedNotification("~b~" + CallSign + ",~y~ many gang members are en route. Be prepared for their arrival.", "CHAR_CALL911", "CHAR_CALL911", "Dispatch", "Pursuit", 15f);
        FivePD.API.Utilities.RequestBackup(Utilities.Backups.Code99);
    }

    public async override void OnStart(Ped player)
    {
        base.OnStart(player);
        suspect1 = await SpawnPed(PedHash.MexGang01GMY, World.GetNextPositionOnStreet(Location + 60));
        suspect2 = await SpawnPed(PedHash.MexGang01GMY, World.GetNextPositionOnStreet(Location + 60));
        suspect3 = await SpawnPed(PedHash.MexGang01GMY, World.GetNextPositionOnStreet(Location + 60));
        suspect4 = await SpawnPed(PedHash.MexGang01GMY, World.GetNextPositionOnStreet(Location + 60));

        van1 = await SpawnVehicle(VehicleHash.GBurrito2, World.GetNextPositionOnStreet(Location + 80));
        driver1 = await SpawnPed(PedHash.MexGang01GMY, World.GetNextPositionOnStreet(Location + 62));
        passenger1 = await SpawnPed(PedHash.MexGang01GMY, World.GetNextPositionOnStreet(Location + 64));

        driver1.SetIntoVehicle(van1, VehicleSeat.Driver);
        passenger1.SetIntoVehicle(van1, VehicleSeat.Passenger);

        driver1.Task.DriveTo(van1, Location, 5, 200);
        passenger1.Weapons.Give(WeaponHash.APPistol, 9999, true, true);
        passenger1.Task.ShootAt(player);

        suspect1.Task.FightAgainst(player);
        suspect2.Task.FightAgainst(player);
        suspect3.Task.FightAgainst(player);
        suspect4.Task.FightAgainst(player);

        suspect1.Weapons.Give(WeaponHash.AssaultRifleMk2, 9999, true, true);
        suspect2.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
        suspect3.Weapons.Give(WeaponHash.VintagePistol, 9999, true, true);
        suspect4.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);

        suspect1.AttachBlip();
        suspect2.AttachBlip();
        suspect3.AttachBlip();
        suspect4.AttachBlip();
        driver1.AttachBlip();
        passenger1.AttachBlip();
        van1.AttachBlip();

        suspect1.AlwaysKeepTask = true;
        suspect2.AlwaysKeepTask = true;
        suspect3.AlwaysKeepTask = true;
        suspect4.AlwaysKeepTask = true;

        passenger1.AlwaysKeepTask = true;
        driver1.AlwaysKeepTask = true;

        suspect1.BlockPermanentEvents = true;
        suspect2.BlockPermanentEvents = true;
        suspect3.BlockPermanentEvents = true;
        suspect4.BlockPermanentEvents = true;

        passenger1.BlockPermanentEvents = true;
        driver1.BlockPermanentEvents = true;

        API.SetPedRelationshipGroupHash(suspect1.NetworkId, 6);
        API.SetPedRelationshipGroupHash(suspect2.NetworkId, 6);
        API.SetPedRelationshipGroupHash(suspect3.NetworkId, 6);
        API.SetPedRelationshipGroupHash(suspect4.NetworkId, 6);


        API.SetPedRelationshipGroupHash(driver1.NetworkId, 6);
        API.SetPedRelationshipGroupHash(passenger1.NetworkId, 6);

    }
}