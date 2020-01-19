using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix
{
    private int rows, columns;
    private float[,] matrix;

    public Matrix(int rows, int columns)
    {
        this.rows = rows;
        this.columns = columns;
        matrix = new float[rows, columns];
    }

    public void Randomize()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                matrix[i, j] = Random.Range(-1f,1f);
            }
        }
    }

    public static Matrix ColumnMatrixFromArray(float[] array)
    {
        Matrix new_matrix = new Matrix(array.Length, 1);

        for (int i = 0; i < new_matrix.Rows; i++)
        {
            new_matrix.M[i, 0] = array[i];
        }

        return new_matrix;
    }

    public Matrix MultiplyByMatrix(Matrix to_multiply)
    {
        Matrix result = new Matrix(rows, to_multiply.Columns);

        if (columns == to_multiply.Rows)
        {
            float sum;
            for (int r = 0; r < rows; r++)
            {
                for (int tm_c = 0; tm_c < to_multiply.Columns; tm_c++)
                {
                    sum = 0;

                    for (int c = 0; c < columns; c++)
                    {
                        sum += matrix[r, c] * to_multiply.M[c, tm_c];
                    }

                    result.M[r, tm_c] = sum;
                }
            }
        }

        return result;
    }

    public void AddBias()
    {
        Matrix with_bias = new Matrix(rows + 1, 1);

        for (int r = 0; r < rows; r++)
        {
            with_bias.M[r, 0] = matrix[r, 0];
        }

        with_bias.M[rows, 0] = 1;

        matrix = with_bias.M;
        columns = with_bias.Columns;
        rows = with_bias.Rows;
    }

    public Matrix NewMatrixWithBias()
    {
        Matrix with_bias = new Matrix(rows + 1, 1);

        for (int r = 0; r < rows; r++)
        {
            with_bias.M[r, 0] = matrix[r, 0];
        }

        with_bias.M[rows, 0] = 1;

        return with_bias;
    }

    public void Activate()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                matrix[i, j] = Maths.Sigmoid(matrix[i, j]);
            }
        }
    }

    public Matrix NewMatrixActivated()
    {
        Matrix activated_matrix = new Matrix(rows, columns);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                activated_matrix.M[i, j] = Maths.Sigmoid(matrix[i, j]);
            }
        }

        return activated_matrix;
    }

    public Matrix Clone()
    {
        Matrix clone = new Matrix(rows, columns);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                clone.M[i, j] = matrix[i, j];
            }
        }

        return clone;
    }

    public void Mutate()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (Random.value < MindConstants.MUTATION_RATE)
                {
                    matrix[i, j] += Maths.RandomNormal();

                    if (matrix[i, j] > 1)
                        matrix[i, j] = 1;
                    else if (matrix[i, j] < -1)
                        matrix[i, j] = -1;
                }
            }
        }
    }


    public float[] ToArray()
    {
        float[] array = new float[rows*columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                array[j+i*columns] = matrix[i,j];
            }
        }

        return array;
    }

    public string ToCSV()
    {
        string s = "";

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                s += ""+matrix[i, j];
                if (j != columns-1)
                    s += ":";
            }
            if (i != rows - 1)
                s += ";";
        }

        return s;
    }

    public void LoadFromCSV(string csv)
    {
        string[] r = csv.Split(';');
        string[] c;

        for (int i = 0; i < r.Length; i++)
        {
            c = r[i].Split(':');

            for (int j = 0; j < c.Length; j++)
            {
                matrix[i, j] = float.Parse(c[j]);
            }
        }
    }


    public int Rows { get => rows; set => rows = value; }
    public int Columns { get => columns; set => columns = value; }
    public float[,] M { get => matrix; set => matrix = value; }
}
