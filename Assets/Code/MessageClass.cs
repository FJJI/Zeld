﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{
    public string from;
    public string content;

    public Message(string from, string content)
    {
        this.from = from;
        this.content = content;
    }
}
