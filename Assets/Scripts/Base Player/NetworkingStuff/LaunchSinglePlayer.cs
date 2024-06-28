using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Alteruna{
public class LaunchSinglePlayer : CommunicationBridge
{
    public void OnClick(){
        Multiplayer.CreatePrivateRoom("singleplayer",1,true,true);
    }
}
}

