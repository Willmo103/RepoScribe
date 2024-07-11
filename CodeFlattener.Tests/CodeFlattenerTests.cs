namespace CodeFlattener.Tests
{
    public class CodeFlattenerTests : IDisposable
    {
        private readonly string testDir;
        private readonly string outputFile;

        public CodeFlattenerTests()
        {
            testDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(testDir);
            outputFile = Path.Combine(testDir, "output.md");

            // Create test files
            Directory.CreateDirectory(Path.Combine(testDir, "subfolder"));
            File.WriteAllText(Path.Combine(testDir, "test.cs"), "Console.WriteLine(\"Hello, World!\");");
            File.WriteAllText(Path.Combine(testDir, "subfolder", "test.js"), "console.log(\"Hello, World!\");");
        }

        public void Dispose()
        {
            Directory.Delete(testDir, true);
        }

        [Fact]
        public void FlattenCodebase_CreatesCorrectMarkdownContent()
        {
            // Arrange
            var flattener = new CodeFlattener();

            // Act
            flattener.FlattenCodebase(testDir, outputFile);

            // Assert
            string content = File.ReadAllText(outputFile);
            Assert.Contains("# test.cs", content);
            Assert.Contains("```csharp", content);
            Assert.Contains("Console.WriteLine(\"Hello, World!\");", content);
            Assert.Contains("# subfolder/test.js", content);
            Assert.Contains("```javascript", content);
            Assert.Contains("console.log(\"Hello, World!\");", content);
        }

        [Theory]
        [InlineData("test.cs", "csharp")]
        [InlineData("test.js", "javascript")]
        [InlineData("test.txt", "plaintext")]
        public void GetLanguageIdentifier_ReturnsCorrectIdentifier(string fileName, string expectedIdentifier)
        {
            // Act
            string result = FileHelper.GetLanguageIdentifier(fileName);

            // Assert
            Assert.Equal(expectedIdentifier, result);
        }

        [Fact]
        public void Main_WithValidArguments_FlattensCodabase()
        {
            // Arrange
            string[] args = new string[] { testDir, outputFile };

            // Act
            Program.Main(args);

            // Assert
            Assert.True(File.Exists(outputFile));
            string content = File.ReadAllText(outputFile);
            Assert.Contains("# test.cs", content);
            Assert.Contains("# subfolder/test.js", content);
        }

        [Fact(Timeout = 10000)] // 10 second timeout
        public void Main_WithInvalidArguments_DoesNotFlattenCodebase()
        {
            // Arrange
            string[] args = new string[] { "invalid_path", outputFile };

            // Act & Assert
            var exception = Assert.Throws<DirectoryNotFoundException>(() => Program.Main(args));
            Assert.Contains("Directory not found: invalid_path", exception.Message);
        }

        [Fact]
        public void Main_WithFileExtensions_OnlyFlattensSpecifiedFiles()
        {
            // Arrange
            string[] args = new string[] { testDir, outputFile, ".cs" };

            // Act
            Program.Main(args);

            // Assert
            Assert.True(File.Exists(outputFile));
            string content = File.ReadAllText(outputFile);
            Assert.Contains("# test.cs", content);
            Assert.DoesNotContain("# subfolder/test.js", content);
        }
        [Fact]
        public void FlattenCodebase_WithIgnoredPaths_DoesNotIncludeIgnoredFiles()
        {
            // Arrange
            var flattener = new CodeFlattener();
            string[] ignoredPaths = { "ignored_folder", "ignored_file.txt" };

            // Create test files including ignored ones
            File.WriteAllText(Path.Combine(testDir, "test.cs"), "Console.WriteLine(\"Hello, World!\");");
            File.WriteAllText(Path.Combine(testDir, "ignored_file.txt"), "This should be ignored");
            Directory.CreateDirectory(Path.Combine(testDir, "ignored_folder"));
            File.WriteAllText(Path.Combine(testDir, "ignored_folder", "ignored.cs"), "This should be ignored");

            // Act
            flattener.FlattenCodebase(testDir, outputFile, null, ignoredPaths);

            // Assert
            string content = File.ReadAllText(outputFile);
            Assert.Contains("# test.cs", content);
            Assert.DoesNotContain("ignored_file.txt", content);
            Assert.DoesNotContain("ignored_folder", content);
        }
    }
}