using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Input
{
    public class ArduinoInput : MonoBehaviour
    {
        [SerializeField]
        private int baudRate;

        [SerializeField]
        private string portName;

        [SerializeField]
        private Vector2 intensityRange;

        [SerializeField]
        private AnimationCurve intensityMapCurve;

        [SerializeField]
        private int signalQueueSize;

        [SerializeField]
        private bool log;

        [SerializeField]
        private bool connectOnStart;

        public UnityEvent<float> OnIntensityChange;

        public UnityEvent<string> OnStatusChange;
        public static ArduinoInput Instance { get; private set; }

        private readonly Queue<float> signalQueue = new();

        private SerialPort serialPort;

        private List<float> signalBuffer;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (connectOnStart)
            {
                Connect();
            }
        }

        private void Update()
        {
            if (serialPort == null || !serialPort.IsOpen)
            {
                return;
            }

            if (serialPort.BytesToRead <= 0)
            {
                return;
            }

            float signal = float.Parse(serialPort.ReadLine());

            signalQueue.Enqueue(signal);

            if (signalQueue.Count > signalQueueSize)
            {
                signalQueue.Dequeue();
            }

            // Use the highest signal as the accepted value
            float highestSignal = signalQueue.Max();
            float normalizedSignal = Mathf.InverseLerp(intensityRange.x, intensityRange.y, highestSignal);
            float mappedSignal = intensityMapCurve.Evaluate(normalizedSignal);

            OnIntensityChange?.Invoke(mappedSignal);

            if (log)
            {
                Debug.Log($"Signal: {signal}, Normalized: {normalizedSignal}, Mapped: {mappedSignal}");
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        public void Disconnect()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        public void Connect()
        {
            Disconnect();

            try
            {
                Debug.Log($"Using port: {portName}");
                serialPort = new SerialPort(portName, baudRate);
                serialPort.ReadTimeout = 100;
                serialPort.Open();
                OnStatusChange?.Invoke($"Connected to {portName}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to open serial port: {e.Message}");
                OnStatusChange?.Invoke($"Error Connecting: {e.Message}");
            }
        }

        public void SetSerialPort(string newPortName)
        {
            portName = newPortName;
        }
    }
}