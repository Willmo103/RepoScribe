Sure, here is a README for your project:

---

# FlattenCodeBase

FlattenCodeBase is a utility for consolidating and organizing code files from a directory into a single Markdown document. This tool is particularly useful for creating comprehensive documentation or overviews of codebases by extracting and compressing the contents of code files.

## Features

- **Path Filtering:** Exclude files from specific directories using configurable ignored paths.
- **File Type Filtering:** Process only specified types of files using configurable allowed file types.
- **Content Compression:** Optionally compress the content of code files by removing unnecessary whitespace.
- **Language Identification:** Automatically identify the programming language of code files for syntax highlighting in Markdown.
- **Comprehensive Error Handling:** Provides detailed error messages and stack traces for easier debugging.

## Requirements

- .NET 6.0 SDK or higher
- A configuration file named `appsettings.json` in the project's root directory.

## Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/Willmo103/More-console-Apps.git
   ```

2. Navigate to the project directory:
   ```sh
   cd More-console-Apps/FlattenCodeBase
   ```

3. Restore dependencies:
   ```sh
   dotnet restore
   ```

4. Build the project:
   ```sh
   dotnet build
   ```

## Configuration

Create an `appsettings.json` file in the project root directory with the following structure:

```json
{
  "AllowedFiles": {
    ".cs": "csharp",
    ".js": "javascript",
    ".ts": "typescript",
    ".html": "html",
    ".css": "css",
    ".scss": "scss",
    ".json": "json",
    ".xml": "xml",
    ".yml": "yaml",
    ".yaml": "yaml",
    ".md": "markdown",
    ".txt": "plaintext",
    ".csproj": "xml",
    ".csv": "csv",
    ".sql": "sql",
    ".sh": "shellscript",
    ".bat": "batch",
    ".ps1": "powershell",
    ".psm1": "powershell",
    ".psd1": "powershell",
    ".ps1xml": "powershell",
    ".xaml": "xaml",
    ".config": "xml",
    ".gitignore": "gitignore",
    ".editorconfig": "editorconfig",
    ".dockerignore": "dockerignore",
    ".dockerfile": "dockerfile",
    ".sln": "xml"
  },
  "Ignored": {
    ".git": "Anything in the .git folder",
    "node_modules": "node_modules",
    "bin": "bin",
    "obj": "obj",
    ".env": "environment files",
    "package-lock.json": "package-lock.json",
    ".vs": "vs",
    ".vscode": "vscode",
    ".idea": "idea",
    "__pycache__": "Python cache files",
    ".pytest_cache": "pytest cache files",
    ".mypy_cache": "mypy cache files",
    ".tox": "tox",
    ".venv": "venv",
    "venv": "venv",
    ".vscode-test": "vscode-test"
  }
}
```

## Usage

To run the FlattenCodeBase tool, use the following command:

```sh
dotnet run -- <rootFolder> <outputFile> [-c|-Compress]
```

- `<rootFolder>`: The root directory containing the code files to be processed.
- `<outputFile>`: The path to the output Markdown file.
- `[-c|-Compress]` (optional): Flag to enable content compression.

### Example

```sh
dotnet run -- "C:\Projects\MyCodeBase" "C:\Projects\MyCodeBase\Documentation.md" -c
```

## Contributing

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/your-feature`).
3. Make your changes.
4. Commit your changes (`git commit -am 'Add some feature'`).
5. Push to the branch (`git push origin feature/your-feature`).
6. Create a new Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgements

- [Newtonsoft.Json](https://www.newtonsoft.com/json) for JSON parsing.
- [Microsoft.Extensions.Configuration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration) for configuration handling.

## Contact

For any questions or issues, please open an issue on GitHub or contact @Willmo103.
