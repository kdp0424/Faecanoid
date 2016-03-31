using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Class that will automatically start and stop coroutines by assigning entries to a dictionary
public class CoroutineManager : Singleton<CoroutineManager>
{    
    private static Utility.TranslateDictionary<string, Item> routines = new Utility.TranslateDictionary<string, Item>();    
    
    // Use this to start a coroutine, and stop any coroutine that previously was assigned to this entry.
    public static void Assign(string name, IEnumerator routine)
    {
        //Debug.Log("CoroutineManager Assign  " + name + "  called at " + Time.realtimeSinceStartup);
        routines[name].sequence = routine;        

    }

    //Use this directly if you don't want to add an entry to the dictionary and just want a managed coroutine item.
    public class Item
    {
        public bool isRunning { get; private set; }
        private MonoBehaviour objectRunningThis = null;
        private IEnumerator _sequence = null;

        public bool autoStart = true;
        //Assign a value to this to automatically stop the previous coroutine and start the next one.
        public IEnumerator sequence
        {
            get
            {
                return _sequence;
            }
            set
            {
                Stop();
                //if (sequence == null) return;
                _sequence = RadicalRoutine.Run(value);
                if(value != null && autoStart) Start();
            }
        }

        public Item()
        {

        }

        public Item(IEnumerator newSequence, MonoBehaviour behavior = null)
        {
            _sequence = newSequence;
            objectRunningThis = behavior;
        }

        public Item(IEnumerator newSequence, bool autoStart, MonoBehaviour behavior = null)
        {
            this.autoStart = autoStart;
            _sequence = newSequence;
            objectRunningThis = behavior;
        }

        public void Start()
        {
            if (isRunning) return;
            
            //Adds RadicalRoutine wrapper to the coroutine so that extended yields such as WaitForUnscaledSeconds may be used.
            if (_sequence != null && autoStart)
            {
                if (objectRunningThis != null)
                {
                    objectRunningThis.StartCoroutine(_sequence);
                }
                else
                {
                    CoroutineManager.instance.StartCoroutine(_sequence);
                }
                isRunning = true;
            }            
        }

        public void Stop()
        {

            if (_sequence != null)
            {
                if (objectRunningThis != null)
                {
                    objectRunningThis.StopCoroutine(_sequence);
                }
                else
                {
                    CoroutineManager.instance.StopCoroutine(_sequence);
                }
            }
            isRunning = false;
        }

        public class Collection
        {
            private List<Item> items = new List<Item>();

            public void Add(Item item)
            {
                items.Add(item);
            }

            public void Remove(Item item)
            {
                items.Remove(item);
            }

            public void Start()
            {
                for(int n = 0; n < items.Count; n++)
                {
                    items[n].Start();
                }
            }

            public void Stop()
            {
                for (int n = 0; n < items.Count; n++)
                {
                    items[n].Stop();
                }
            }
        }
    }
}