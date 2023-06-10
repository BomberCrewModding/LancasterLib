using BepInEx;
using BepInEx.Logging;
using Lancaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExampleMod;

[BepInPlugin("com.bcmodding.example_mod", "Example mod", "1.0.0")]
public class Main : BaseUnityPlugin
{
    private void Awake()
    {
        Logger.LogInfo("Initializing example plugin");

        /*
         * Let's add a new upgrade !
         */

    }
}
