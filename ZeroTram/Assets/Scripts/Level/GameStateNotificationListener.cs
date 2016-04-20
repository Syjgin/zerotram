using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface GameStateNotificationListener
{
	void UpdateInfo(GameController.StateInformation information);
	void GameOver();
}
