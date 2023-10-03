using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.JSON;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

public class UdonSharpHelper : UdonSharpBehaviour
{
    /// <summary>
    /// Serialises a DataList to Json
    /// </summary>
    /// <param name="dataList"></param>
    /// <returns></returns>
    public static string SerialiseToJson(DataList dataList)
    {
        if (VRCJson.TrySerializeToJson(dataList, JsonExportType.Minify, out DataToken result))
        {
            return result.String;
        }
        else
        {
            Debug.LogError(result.ToString());
            return null;
        }
    }

    /// <summary>
    /// Deserialises a DataList from a json to DataList
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static DataList DeserialiseDatalist(string json)
    {
        if (VRCJson.TryDeserializeFromJson(json, out DataToken result))
        {
            return result.DataList;
        }
        else
        {
            Debug.LogError(result.ToString());
            return null;
        }
    }
}
