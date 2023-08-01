using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Reflection;

namespace parserTester
{
    public class ParserIndeedTester
    {
        private ScriptEngine engine;
        private ScriptScope scope;

        public ParserIndeedTester()
        {
            // Get the location of the .dll that Visual Studio is producing
            string assemblyFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Move up to the solution root
            string solutionDirectory = Directory.GetParent(assemblyFolderPath).Parent.Parent.Parent.FullName;
            string pythonProjectSrc = $"{solutionDirectory}\\pythonFunc\\jobParserIndeedCom";
            string pythonFunctSrc = $"{pythonProjectSrc}\\jobParserIndeedCom.py";

            engine = Python.CreateEngine(); // Extract Python language engine from their grasp
            ICollection<string> searchPaths = engine.GetSearchPaths();

            searchPaths.Add(pythonProjectSrc);
            engine.SetSearchPaths(searchPaths);

            var scriptSource = engine.CreateScriptSourceFromFile(pythonFunctSrc);
            scope = engine.CreateScope();
            scriptSource.Execute(scope);
        }

        [Fact]
        public void canGetContentOfTheUrl()
        {
            var testUrl = "\"https://test.com\"";
            dynamic getJobDetailsFunc = scope.GetVariable("get_job_details");
            var result = getJobDetailsFunc(testUrl);  // Call Python function

            Assert.NotEmpty(result);
        }
    }
}