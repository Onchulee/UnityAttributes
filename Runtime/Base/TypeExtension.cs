public static class TypeExtension
{
    public static System.Reflection.FieldInfo GetFieldViaPath(this System.Type type,
        string path)
    {
        var parent = type;
        var fi = parent.GetField(path);
        var paths = path.Split('.');

        for (int i = 0; i < paths.Length; i++)
        {
            fi = parent.GetField(paths[i]);

            // there are only two container field type that can be serialized:
            // Array and List<T>
            if (fi.FieldType.IsArray)
            {
                parent = fi.FieldType.GetElementType();
                i += 2;
                continue;
            }

            if (fi.FieldType.IsGenericType)
            {
                parent = fi.FieldType.GetGenericArguments()[0];
                i += 2;
                continue;
            }

            if (fi != null)
            {
                parent = fi.FieldType;
            }
            else
            {
                return null;
            }

        }

        return fi;
    }
}