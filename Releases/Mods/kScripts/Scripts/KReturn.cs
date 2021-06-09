using System.Collections.Generic;


namespace kScripts
{
    class KReturn : ConsoleCmdAbstract
	{
    
		private EntityPlayer _entityPlayer;

		public override string[] GetCommands() => new[] { "KReturn" };

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
				_entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();
				KTeleportObject teleportObject = new KTeleportObject();

				if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
				{
					Vector3i returnV3I = _entityPlayer.GetBlockPosition();

					if (teleportObject.TryGetLocation("return", out var targetV3I))
                    {
						teleportObject.Add("return", returnV3I);
						KHelper.Teleport(targetV3I);
					} else
                    {
						KHelper.ChatOutput(_entityPlayer, "No return location was stored.");
                    }

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
	}
}
