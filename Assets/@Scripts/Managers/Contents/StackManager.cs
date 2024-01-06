using Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StackManager
{
    public HashSet<TokenController> Tokens { get; } = new HashSet<TokenController>();
    
    
}
