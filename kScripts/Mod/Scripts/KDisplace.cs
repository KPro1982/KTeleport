
using System;
using System.Linq;
using System.Collections.Generic;
namespace kScripts
{ 
	public class k_displace : ConsoleCmdAbstract
	  {
		  private EntityPlayer entityPlayer;
		  private Vector3i newLocationV3i = new Vector3i(0,0,0);
		  private int dx, dz;

		  public override string[] GetCommands() => new[] {"k_displace"};

		  public override string GetDescription() => "Get location of player entity.";

		  public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
		{
			if (_params.Count == 2 )
			{
			
				bool isValid = int.TryParse(_params[0], out dx);
				if (isValid)
				{

					isValid = int.TryParse(_params[1], out dz);
				}
			} else
			{
				var rand = new Random();
				dx = rand.Next(10, 30);
				dz = rand.Next(10, 30);
			}

				if (!_senderInfo.IsLocalGame && _senderInfo.RemoteClientInfo == null)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command can only be used on clients.");
				return;
			}

		
			if (_senderInfo.IsLocalGame)
			{
				entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();
				newLocationV3i = displaceEntity(dx, 0, dz);
				kHelper.ChatOutput(entityPlayer, string.Format("Current Player Coordinates: {0},{1}  -> New Coordinates {2}, {3}", entityPlayer.GetBlockPosition().x, entityPlayer.GetBlockPosition().z, newLocationV3i.x, newLocationV3i.z));
				if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
				{
					kHelper.Teleport(newLocationV3i);
				}
				else
				{
					//SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageConsoleCmdServer>().Setup(GameManager.Instance.World.GetPrimaryPlayerId(), "your command"), false);
				}

			}
			else
			{
				entityPlayer = GameManager.Instance.World.Players.dict[_senderInfo.RemoteClientInfo.entityId];
			}


		}
		private Vector3i  displaceEntity(int _x, int _y, int _z)
		{
			Vector3i displaceAmount = new Vector3i(_x, _y, _z);
			return entityPlayer.GetBlockPosition() + displaceAmount;
		}
	
	

        
	 }
   
}
