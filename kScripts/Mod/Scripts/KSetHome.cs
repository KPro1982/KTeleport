
using System;
using System.Linq;
using System.Collections.Generic;


namespace kScripts
{
	public class k_sethome : ConsoleCmdAbstract
	{
		private EntityPlayer entityPlayer;
		public kTeleportObject saveTeleport = new kTeleportObject();

		public override string[] GetCommands() => new[] { "k_sethome" };

		public override string GetDescription() => "Set home base location to be used with k_gohome.";

		public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
		{

			
			if (!_senderInfo.IsLocalGame && _senderInfo.RemoteClientInfo == null)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command can only be used on clients.");
				return;
			}


			if (_senderInfo.IsLocalGame)
			{

				if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
				{
					entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();
					saveTeleport.Add("home", entityPlayer.GetBlockPosition());
					kHelper.ChatOutput(entityPlayer, "Home location stored.");
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
		void SaveLocation(string _name, Vector3i _location)
		{
			saveTeleport.Add(_name, _location);
			return;

		}



	}


}