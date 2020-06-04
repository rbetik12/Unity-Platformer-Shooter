﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Net {
    public class ThreadManager: MonoBehaviour {
        private static readonly List<Action> executeOnMainThread = new List<Action>();
        private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
        private static bool _actionToExecuteOnMainThread;

        private void Update() {
            UpdateMain();
        }

        /// <summary>Sets an action to be executed on the main thread.</summary>
        /// <param name="_action">The action to be executed on the main thread.</param>
        public static void ExecuteOnMainThread(Action _action) {
            if (_action == null) {
                Debug.Log("No action to execute on main thread!");
                return;
            }

            lock (executeOnMainThread) {
                executeOnMainThread.Add(_action);
                _actionToExecuteOnMainThread = true;
            }
        }

        /// <summary>Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.</summary>
        public static void UpdateMain() {
            if (_actionToExecuteOnMainThread) {
                executeCopiedOnMainThread.Clear();
                lock (executeOnMainThread) {
                    executeCopiedOnMainThread.AddRange(executeOnMainThread);
                    executeOnMainThread.Clear();
                    _actionToExecuteOnMainThread = false;
                }

                for (int i = 0; i < executeCopiedOnMainThread.Count; i++) {
                    executeCopiedOnMainThread[i]();
                }
            }
        }
    }
}