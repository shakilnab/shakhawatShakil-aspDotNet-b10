using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JsonFormattingAssignment;

public class JsonFormatter
{
    public static string Convert(object item)
    {
        /*=============================================================================
         1. If item is null then, it will return null, if not null then using 
            item.GetType() itemType will hold the type of object. Then sequentially
            serialize this.
        =============================================================================*/
        if (item == null)
            return "null";
        Type itemType = item.GetType();

        /*=============================================================================
         1. This condition check whether itemType is primitive or not.
            In this serializer we only consider int, double, decimal, bool, 
            string, char, DateTime data type.
         2. If itemType satisfy the condition then it calls FormatPrimitiveType method 
            to format each data type in JSON format.            
        =============================================================================*/
        if (itemType.IsPrimitive || itemType == typeof(decimal) ||
            itemType == typeof(string) || itemType == typeof(DateTime))
        {
            return FormatPrimitiveType(item);
        }

        /*=============================================================================
         1. This condition check whether itemType is Array or List or not one of these.
         2. If itemType satisfy the condition then it calls FormatArrayType method 
            to format Array and List in JSON format.            
        =============================================================================*/
        else if (itemType.IsArray || (item is IList))
        {
            return FormatArrayType(item);
        }

        /*=============================================================================
         1. This condition check whether itemType is Class or not.
         2. If itemType satisfy the condition then it calls FormatObjectType method 
            to format Object in JSON format.            
        =============================================================================*/
        else if (itemType.IsClass)
        {
            return FormatObjectType(item);
        }

        /*=============================================================================
         1. If itemType does not satisfy above three, then it means the serializer can
            not handle this types. Out of constraints.
         2. Then it will throw exception.            
        =============================================================================*/
        else
        {
            throw new ArgumentException("This serializer can not handle this type.");
        }
    }


    /*=============================================================================
     1. This FormatPrimitiveType method will handle all primitive data type under 
        given constraints.          
    =============================================================================*/
    private static string FormatPrimitiveType(object item)
    {
        if (item is DateTime)
        {
            return $"\"{((DateTime)item).ToString("dd/MM/yyyy HH:mm:ss")}\"";
        }
        else if (item is string || item is char)
        {
            return $"\"{item}\"";
        }
        else
        {
            return item.ToString().ToLower();
        }
    }


    /*=============================================================================
     1. This FormatArrayType method will handle all Array(1D and jagged array) and 
        List by recursively calling Convert method.        
    =============================================================================*/
    private static string FormatArrayType(object item)
    {
        StringBuilder sb = new StringBuilder();
        IList list = (IList)item;

        sb.Append("[");
        for (int i = 0; i < list.Count; i++)
        {
            sb.Append(Convert(list[i]));
            if (i < list.Count - 1)
                sb.Append(",");
        }
        sb.Append("]");

        return sb.ToString();
    }


    /*=============================================================================
     1. This FormatObjectType method will handle given object.
     2. It will recursively call convert method for proper formatting of each
        property value.
    =============================================================================*/
    private static string FormatObjectType(object item)
    {
        StringBuilder sb = new StringBuilder();
        Type itemType = item.GetType();

        sb.Append("{");

        PropertyInfo[] properties = itemType.GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo property = properties[i];
            sb.Append($"\"{property.Name}\":{Convert(property.GetValue(item))}");
            if (i < properties.Length - 1)
                sb.Append(",");
        }

        sb.Append("}");

        return sb.ToString();
    }
}
