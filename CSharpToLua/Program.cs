using System;
using LuaInterface;
using System.Reflection;

namespace CSharpToLua {
    class Program {
        static string luaChunk = @"
                                function luaFunc(i)
                                    print('hello it is '..i);
                                    return i,i*2,i*4;
                                end";
          static string luaChunk2 = @"
                                lua1();
                                ";
                                

        static void Main(string[] args) {
            LuaFramework luaVM = new LuaFramework();
            luaVM.DoString(luaChunk);
            object[] objs = luaVM.Call("luaFunc", 7);
            
            luaVM.BindLuaApiClass(new LuaAPI());
            luaVM.DoString(luaChunk2);
            Console.Read();
        }   
    }
    
    public class LuaFunction : Attribute  
    {  
        private String FunctionName;  
  
        public LuaFunction(String strFuncName)  
        {  
            FunctionName = strFuncName;  
        }  
  
        public String getFuncName()  
        {  
            return FunctionName;  
        }  
    }  
    class LuaFramework  
    {  
        private Lua pLuaVM = new Lua();//lua虚拟机  
  
        /// <summary>  
        /// 注册lua函数  
        /// </summary>  
        /// <param name="pLuaAPIClass">lua函数类</param>  
        public void BindLuaApiClass(Object pLuaAPIClass)  
        {  
            foreach (MethodInfo mInfo in pLuaAPIClass.GetType().GetMethods())  
            {  
                foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))  
                {  
                    LuaFunction func = attr as LuaFunction;
                    if (func != null) { 
                        string LuaFunctionName = func.getFuncName(); 
                        pLuaVM.RegisterFunction(LuaFunctionName, pLuaAPIClass, mInfo);  
                    }
                }  
            }  
        }  
  
        /// <summary>  
        /// 加载lua脚本文件  
        /// </summary>  
        /// <param name="luaFileName">脚本文件名</param>  
        public void DoFile(string luaFileName)  
        {  
            try  
            {  
                pLuaVM.DoFile(luaFileName);  
            }  
            catch (Exception e)  
            {  
                Console.Write(e.Message);
            }  
        }  
  
        /// <summary>  
        /// 加载lua脚本  
        /// </summary>  
        /// <param name="luaCommand">lua指令</param>  
        public void DoString(string luaCommand)  
        {  
            try  
            {  
                pLuaVM.DoString(luaCommand);  
            }  
            catch (Exception e)  
            {  
                Console.Write(e.Message);
            }  
        }

        public object[] Call(string func, int param) {
            return pLuaVM.GetFunction("luaFunc").Call(param);
        }
    }  

    class LuaAPI  
    {  
        [LuaFunction("lua1")]  
        public void a1()  
        {  
            Console.Write("a1 called");  
        }
    }  
}
