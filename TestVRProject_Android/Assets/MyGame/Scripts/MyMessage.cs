using UnityEngine;
using System.Collections;
using Mirror;

public class MyMessage : MessageBase
{
    public uint networkInstanceID; //see https://vis2k.github.io/Mirror/General/Migration.html
    public float steeringInput;
    public float motorInput;
    public bool breakInput;
    public string text;
}