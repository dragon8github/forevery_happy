using System;
using System.Reflection;
using System.Text;

namespace ConsoleApp
{
    public enum GenderEnum
    {
        Boy = 0,
        Girl = 1,
        Dog = 2
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode; // 输入
            Console.InputEncoding = Encoding.Unicode; // 输出
            // ==============================================

            WorkerModel worker = new WorkerModel() {
                Name = "Jay",
                Gender = GenderEnum.Boy,
            };
            Type type = worker.GetType();
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (var f in fieldInfos) {
                string fieldName = f.Name;
                string fieldType = f.FieldType.ToString();
                string fieldValue = f.GetValue(worker).ToString();
                Console.WriteLine("字段名 = {0} | 字段类型 = {1} | 字段值 = {2}", fieldName, fieldType, fieldValue);
            }

            //GetFieldValue(worker);
            //GetPropertyInfo(worker);

            // ==============================================
            Console.ReadLine();
        }

        /// <summary>
        /// 字段信息
        /// </summary>
        /// <param name="obj"></param>
        public static void GetFieldValue(Object obj)
        {
            // 得到对象的类型
            Type type = obj.GetType();

            // 得到字段信息，只能得到public类型的字典的值
            //FieldInfo[] fieldInfos = type.GetFields();

            // 得到字段的值,包括private、protected、public的值
            // 以下三个BindingFlags枚举，按顺序称之为1、2、3
            // 1 - NonPublic： 指定非Public修饰的成员
            // 2 - Public：    指定Public修饰的成员
            // 3 - Instance：  指定实例成员将包括在搜索中。
            // 使用特别注意!!!
            // 测试发现: 12组合获取为空, 而13、23组合方式获取到了
            // 原因是官方规定：必须与Public 或 NonPublic 一起指定 Instance 或 Static, 否则将不返回成员。
            /**
             *
             */
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var f in fieldInfos) {
                string fieldName = f.Name;
                string fieldType = f.FieldType.ToString();
                string fieldValue = f.GetValue(obj).ToString();
                Console.WriteLine("字段名 = {0} | 字段类型 = {1} | 字段值 = {2}", fieldName, fieldType, fieldValue);
            }
        }

        /// <summary>
        /// 属性信息
        /// </summary>
        /// <param name="obj"></param>
        public static void GetPropertyInfo(Object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] propertyInfo = type.GetProperties();

            foreach (var p in propertyInfo) {
                string propertyName = p.Name;
                string propertyValue = p.GetValue(obj, null).ToString();
                Console.WriteLine("属性名 = {0} | 属性值 = {1}", propertyName, propertyValue);
            }
        }

        /// <summary>
        /// 方法信息
        /// </summary>
        /// <param name="obj"></param>
        public static void GetMethodInfo(Object obj)
        {
            Type type = obj.GetType();
            //获取所有public修饰的方法
            MethodInfo[] methodInfo = type.GetMethods();

            foreach (var m in methodInfo) {
                string methodName = m.Name;
                Console.WriteLine("方法名 = {0}", methodName);

            }
        }

        /// <summary>
        /// 成员信息
        /// </summary>
        /// <param name="obj"></param>
        public static void GetMemberInfo(Object obj)
        {
            Type type = obj.GetType();
            MemberInfo[] memberInfo = type.GetMembers();

            foreach (var m in memberInfo) {
                string memberName = m.Name;
                Console.WriteLine("成员名 = {0}", memberName);
            }
        }

        /// <summary>
        /// 构造方法信息
        /// </summary>
        /// <param name="obj"></param>
        private static void GetConstructorInfo(Object obj)
        {
            Type type = obj.GetType();
            //获取所有public修饰的构造方法
            ConstructorInfo[] constructorInfo = type.GetConstructors();
            foreach (var c in constructorInfo) {
                string constructorName = c.Name;
                ParameterInfo[] constructorParams = c.GetParameters();
                Console.WriteLine("constructorName------>" + constructorName);

                foreach (var p in constructorParams) {
                    Console.WriteLine("Params------ p.Name-->" + p.Name);
                    Console.WriteLine("Params------ p.Type--->" + p.ParameterType);
                }
            }
        }
    }
    
    public class WorkerModel
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string _name = "Hathway";
        private int _id = 32;
        protected bool _isAdmin = true;
        public GenderEnum _gender = GenderEnum.Girl;

        /// <summary>
        /// 属性
        /// </summary>
        public string Name { get; set; }
        public GenderEnum Gender { get; set; }
        private int Id { get; set; }
        protected bool IsAdmin { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        public void Android() { }
        protected void IOS() { }
        private void WindowPhone() { }

        /// <summary>
        /// 构造方法
        /// </summary>
        public WorkerModel(){ }
        public WorkerModel(string name, int id, GenderEnum gender, bool isAdmin) { }
    }
}
