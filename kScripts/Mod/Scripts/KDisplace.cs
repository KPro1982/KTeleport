
using System;
using System.Collections.Generic;
namespace kScripts
{ 
	public class KDisplace : ConsoleCmdAbstract
	  {
		  private EntityPlayer _entityPlayer;
		  private Vector3i _newLocationV3I = new Vector3i(0,0,0);
		  private int _dx, _dz;

		  public override string[] GetCommands() => new[] {"KDisplace"};

		  public override string GetDescription() => "Get location of player entity.";

		  public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
		{
			if (_params.Count == 2 )
			{
			
				bool isValid = int.TryParse(_params[0], out _dx);
				if (isValid)
				{

					int.TryParse(_params[1], out _dz);
				}
			} else
			{
				var rand = new Random();
				_dx = rand.Next(10, 30);
				_dz = rand.Next(10, 30);
			}

			if (!_senderInfo.IsLocalGame && _senderInfo.RemoteClientInfo == null)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command can only be used on clients.");
				return;
			}

		
			if (_senderInfo.IsLocalGame)
			{
				_entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();
				_newLocationV3I = DisplaceEntity(_dx, 0, _dz);
				KHelper.EasyLog(
					$"Current Player Coordinates: {_entityPlayer.GetBlockPosition().x},{_entityPlayer.GetBlockPosition().z}  -> New Coordinates {_newLocationV3I.x}, {_newLocationV3I.z}",
					LogLevel.Chat);
				if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
				{
					KHelper.Teleport(_entityPlayer, _newLocationV3I, new Vector3i(0,0,0));
				}
				else
				{
					//SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageConsoleCmdServer>().Setup(GameManager.Instance.World.GetPrimaryPlayerId(), "your command"), false);
				}

			}
			else
			{
				_entityPlayer = GameManager.Instance.World.Players.dict[_senderInfo.RemoteClientInfo.entityId];
			}


		}
		private Vector3i  DisplaceEntity(int _x, int _y, int _z)
		{
			Vector3i displaceAmount = new Vector3i(_x, _y, _z);
			return _entityPlayer.GetBlockPosition() + displaceAmount;
		}
	
	

        
	 }
   
}
