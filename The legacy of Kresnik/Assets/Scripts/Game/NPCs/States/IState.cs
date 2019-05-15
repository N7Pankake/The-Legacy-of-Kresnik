using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IState
{ 
    void Enter(Enemy parent);

    void Update();

    void Exit();
}