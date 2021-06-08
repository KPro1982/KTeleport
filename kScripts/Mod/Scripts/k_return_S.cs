using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace kScripts
{
    class k_return : ConsoleCmdAbstract
	{
    
		private EntityPlayer entityPlayer;

		public override string[] GetCommands() => new[] { "k_return" };

		public override string GetDescription() => "Return to location prior to last teleport.";

		public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
		{


			if (!_senderInfo.IsLocalGame && _senderInfo.RemoteClientInfo == null)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command can only be used on clients.");
				return;
			}


			if (_senderInfo.IsLocalGame)
			{
				entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();
				kTeleportObject teleportObject = new kTeleportObject();

				if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
				{
					Vector3i returnV3i = entityPlayer.GetBlockPosition();

					if (teleportObject.TryGetLocation("return", out var targetV3i))
                    {
						teleportObject.Add("return", returnV3i);
						kHelper.Teleport(targetV3i);
					} else
                    {
						kHelper.ChatOutput(entityPlayer, "No return location was stored.");
                    }

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
	}
}
