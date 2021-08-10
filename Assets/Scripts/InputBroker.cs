using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowsInput;
using WindowsInput.Native;

public class InputBroker {
    public void test() {
        InputSimulator i = new InputSimulator();
        i.Keyboard.KeyPress(VirtualKeyCode.SPACE);
    }
}
