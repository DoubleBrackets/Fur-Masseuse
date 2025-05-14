using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent<float> OnIntensityChange;

    private readonly Queue<float> signalQueue = new();

    private SerialPort serialPort;

    private List<float> signalBuffer;

    private void Start()
    {
        string[] portNames = SerialPort.GetPortNames();

        // var portName = portNames.Last();

        Debug.Log($"Using port: {portName}");

        serialPort = new SerialPort(portName, baudRate);

        serialPort.Open();
    }

    private void Update()
    {
        if (serialPort == null || !serialPort.IsOpen)
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
        float highestSignal = signalQueue.Average();
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
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}