using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net
{
    private int input_nodes, output_nodes, hidden_nodes;
    private Matrix output_weights, input_weights;
	private Matrix[] hidden_weights;

	// TODO: Afegir parametrització de les capes ocultes
	public Net(int inputs, int outputs, int hidden, int num_hidden_layers)
    {
        input_nodes = inputs;
        output_nodes = outputs;
        hidden_nodes = hidden;

        input_weights = new Matrix(hidden_nodes, input_nodes + 1);
		hidden_weights = new Matrix[num_hidden_layers];
		for (int i = 0; i < hidden_weights.Length; i++)
			hidden_weights[i] = new Matrix(hidden_nodes, hidden_nodes + 1);
        output_weights = new Matrix(output_nodes, hidden_nodes + 1);
    }

    public void InitializeRandom()
    {
        input_weights.Randomize();
		for (int i = 0; i < hidden_weights.Length; i++)
			hidden_weights[i].Randomize();
        output_weights.Randomize();
    }
    
    public float[] Compute(float[] input)
    {
        Matrix inputs = Matrix.ColumnMatrixFromArray(input);
        inputs.AddBias();

        Matrix layer_1 = input_weights.MultiplyByMatrix(inputs);
        layer_1.Activate();
        layer_1.AddBias();

		Matrix[] hidden_layers = new Matrix[hidden_weights.Length];

		hidden_layers[0] = hidden_weights[0].MultiplyByMatrix(layer_1);
		hidden_layers[0].Activate();
		hidden_layers[0].AddBias();

		for (int i = 1; i < hidden_weights.Length; i++)
		{
			hidden_layers[i] = hidden_weights[i].MultiplyByMatrix(hidden_layers[i-1]);
			hidden_layers[i].Activate();
			hidden_layers[i].AddBias();
		}

        Matrix outputs = output_weights.MultiplyByMatrix(hidden_layers[hidden_weights.Length-1]);
        outputs.Activate();

        return outputs.ToArray();
    }

    public void Mutate()
    {
        input_weights.Mutate();
		for (int i = 0; i < hidden_weights.Length; i++)
			hidden_weights[i].Mutate();
		output_weights.Mutate();
    }

    public Net Clone()
    {
        Net clone = new Net(input_nodes, output_nodes, hidden_nodes, hidden_weights.Length);

        clone.Input_weights = input_weights.Clone();
		clone.Hidden_weights = new Matrix[hidden_weights.Length];
		for (int i = 0; i < hidden_weights.Length; i++)
			clone.Hidden_weights[i] = hidden_weights[i].Clone();
        clone.Output_weights = output_weights.Clone();

        return clone;
    }

    public string SaveToCSV(string file)
    {
		string inputs = input_weights.ToCSV(),
			outputs = output_weights.ToCSV();

        string hidden = "";
		for (int i = 0; i < hidden_weights.Length; i++)
		{
			hidden += hidden_weights[i].ToCSV();
			if (i != hidden_weights.Length - 1)
				hidden += "\n";
		}

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

        for (int i = 2; i < lines.Length; i++)
        {
			hidden_weights[i-2].LoadFromCSV(lines[i]);
		}
    }

    public Matrix Input_weights { get => input_weights; set => input_weights = value; }
    public Matrix Output_weights { get => output_weights; set => output_weights = value; }
    public Matrix[] Hidden_weights { get => hidden_weights; set => hidden_weights = value; }
}
