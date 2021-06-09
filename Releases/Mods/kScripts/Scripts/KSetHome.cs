
using System;
using System.Linq;
using System.Collections.Generic;


namespace kScripts
{
	public class KSetHome : ConsoleCmdAbstract
	{
		private EntityPlayer _entityPlayer;
		public KTeleportObject SaveTeleport = new KTeleportObject();

		public override string[] GetCommands() => new[] { "KSetHome" };

		public override string GetDescription() => "Set home base location to be used with KGoHome.";

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
					_entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();
					SaveTeleport.Add("home", _entityPlayer.GetBlockPosition());
					KHelper.ChatOutput(_entityPlayer, "Home location stored.");
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
		void SaveLocation(string _name, Vector3i _location)
		{
			SaveTeleport.Add(_name, _location);
			return;

		}



	}


}