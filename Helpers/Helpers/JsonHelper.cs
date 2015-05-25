using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace CommonLib.Helpers
{
   public  class JsonHelper
    {
       /// <summary> 
    /// JSON文本转对象,泛型方法 
    /// </summary> 
    /// <typeparam name="T">类型</typeparam> 
    /// <param name="jsonText">JSON文本</param> 
    /// <returns>指定类型的对象</returns> 
    public static T JSONToObject<T>(string jsonText)
    {
       
        JavaScriptSerializer jss =new JavaScriptSerializer();
        try
        {
            return jss.Deserialize<T>(jsonText);
        }
        catch(Exception ex) 
        {
            throw new Exception("JSONHelper.JSONToObject(): "+ ex.Message);
        }
    }

    public static string  ObjectToJSON <T>(T obj)
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        try
        {
            return jss.Serialize(obj);
        }
        catch (Exception ex)
        {
            throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
        }
    }


    }
}
