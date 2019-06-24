using UnityEngine;
using System.Collections;
using Mirror;

public class MyMessage : MessageBase
{
    public uint networkInstanceID; //see https://vis2k.github.io/Mirror/General/Migration.html
    public float steeringInput=0;
    public float motorInput=0;
    public bool breakInput=false;
    public string text="MyMessage";
}