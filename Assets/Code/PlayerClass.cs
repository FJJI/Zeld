﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass
{
    public string username;
    public bool ready;

    public PlayerClass(string username, bool ready)
    {
        this.username = username;
        this.ready = ready;
    }
}
