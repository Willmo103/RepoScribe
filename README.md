# FlattenCodeBase

FlattenCodeBase is a tool for recursively scanning a directory of source code files and flattening them into a single Markdown file. It supports various file formats, ignores specified directories, and offers options for content compression.

### Features:
- Supports multiple file types, including `.cs`, `.js`, `.ts`, `.html`, `.css`, `.json`, `.xml`, `.yml`, `.sql`, `.sh`, and more.
- Ability to ignore specific directories or files like `node_modules`, `.git`, `bin`, `obj`, etc.
- Outputs the flattened code with syntax highlighting in Markdown code blocks.
- Optional compression of file content (removal of spaces).
- Configurable through `appsettings.json` for allowed file types and ignored directories.
- Additional support for Git repository management using commands to add, remove, clone, and list repositories.
  
### Usage:

1. **Basic Command:**
   ```bash
   CodeFlattener --input [root-directory] --output [output-file]
   ```
   This command flattens the codebase from the given input directory to the specified output file in Markdown format.

2. **Compression Option:**
   Add the `--compress` or `-c` flag to enable compression of content within files (removes extra spaces).

3. **Repository Management Commands:**
   - Add a Git repository: `CodeFlattener repo add [url]`
   - Clone a Git repository: `CodeFlattener repo clone [url] --path [optional-path]`
   - List repositories: `CodeFlattener repo list`
   - Remove a repository: `CodeFlattener repo remove [url]`

### Configuration:

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
  }
}
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

## Contact

For any questions or issues, please open an issue on GitHub or contact @Willmo103.
