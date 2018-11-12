using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class SaveFile : MonoBehaviour {

    // private FileStream fs;

    void Start()
    {
        
    }

    public void WriteFile(string fileName, List<string[]> data, string[] str) {
        string filePath = Application.dataPath + fileName;
        bool newData = File.Exists(filePath);

        StreamWriter streamWriter = new StreamWriter(filePath, true, Encoding.GetEncoding("Shift_JIS"));
        string comma_str = string.Join(",", str);
        if (!newData) {
            streamWriter.WriteLine(comma_str);
        }
        for (int i = 1; i < data.Count; i++) {
            comma_str = string.Join(",", data[i]);
            streamWriter.WriteLine(comma_str);
        }
        streamWriter.Close();
    }

    public List<float[]> ReadFile(string fileName) {
        string filePath = Application.dataPath + fileName;
        List < float[] > finishedPattern = new List<float[]>();
        if (!File.Exists(filePath)) {
            
        } else {
            StreamReader streamReader = new StreamReader(filePath, Encoding.GetEncoding("Shift_JIS"));
            string line;
            string[] csvDatas = new string[10];
            bool first = true;
            while ((line = streamReader.ReadLine()) != null)
            {
                if (first)
                {
                    first = false;
                } else {
                    float[] patternData = new float[3];
                    csvDatas = line.Split(',');
                    patternData[0] = float.Parse(csvDatas[0]);
                    patternData[1] = float.Parse(csvDatas[1]);
                    patternData[2] = float.Parse(csvDatas[2]);
                    finishedPattern.Add(patternData);
                }

            }
            streamReader.Close();
        }

        return finishedPattern;
    }

}
