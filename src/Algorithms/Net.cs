using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net
{
    private int input_nodes, output_nodes, hidden_nodes;
    private Matrix input_weights, output_weights, hidden_weights;

    // TODO: Afegir parametrització de les capes ocultes
    public Net(int inputs, int outputs, int hidden)
    {
        input_nodes = inputs;
        output_nodes = outputs;
        hidden_nodes = hidden;

        input_weights = new Matrix(hidden_nodes, input_nodes + 1);
        hidden_weights = new Matrix(hidden_nodes, hidden_nodes + 1);
        output_weights = new Matrix(output_nodes, hidden_nodes + 1);
    }

    public void InitializeRandom()
    {
        input_weights.Randomize();
        hidden_weights.Randomize();
        output_weights.Randomize();
    }
    
    public float[] Compute(float[] input)
    {
        Matrix inputs = Matrix.ColumnMatrixFromArray(input);
        inputs.AddBias();

        Matrix layer_1 = input_weights.MultiplyByMatrix(inputs);
        layer_1.Activate();
        layer_1.AddBias();

        Matrix layer_2 = hidden_weights.MultiplyByMatrix(layer_1);
        layer_2.Activate();
        layer_2.AddBias();

        Matrix outputs = output_weights.MultiplyByMatrix(layer_2);
        outputs.Activate();

        return outputs.ToArray();
    }

    public void Mutate()
    {
        input_weights.Mutate();
        hidden_weights.Mutate();
        output_weights.Mutate();
    }

    public Net Clone()
    {
        Net clone = new Net(input_nodes, output_nodes, hidden_nodes);

        clone.Input_weights = input_weights.Clone();
        clone.Hidden_weights = hidden_weights.Clone();
        clone.Output_weights = output_weights.Clone();

        return clone;
    }

    public string SaveToCSV(string file)
    {
        string inputs = input_weights.ToCSV(),
            outputs = output_weights.ToCSV(),
            hidden = hidden_weights.ToCSV();

        string net = inputs +
            "\n" + outputs +
            "\n" + hidden;

        System.IO.Directory.CreateDirectory(MindConstants.NET_MODELS_PATH);
        System.IO.File.WriteAllText(MindConstants.NET_MODELS_PATH + file, net);

        return net;
    }

    public void LoadFromCSV(string file)
    {
        string[] lines = System.IO.File.ReadAllLines(MindConstants.NET_MODELS_PATH + file);

        input_weights.LoadFromCSV(lines[0]);
        output_weights.LoadFromCSV(lines[1]);
        hidden_weights.LoadFromCSV(lines[2]);

        // TODO
        //for (int i = 2; i < lines.Length; i++)
        //{
            
        //}
    }

    public Matrix Input_weights { get => input_weights; set => input_weights = value; }
    public Matrix Output_weights { get => output_weights; set => output_weights = value; }
    public Matrix Hidden_weights { get => hidden_weights; set => hidden_weights = value; }
}
