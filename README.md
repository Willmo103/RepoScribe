# RepoScribe

RepoScribe is a powerful tool to flatten and document your code repositories into comprehensive Markdown files. It supports various file types, manages ignored paths, and integrates seamlessly with Git repositories.

## Features

- **Flatten Codebase:** Convert your entire codebase into a single Markdown document.
- **Manage Ignored Paths:** Easily add, list, or remove paths that should be ignored during processing.
- **Repository Management:** Add, list, remove, and clone Git repositories.
- **Extract Code Blocks:** Extract code blocks from existing Markdown files with unique identifiers.

## Installation

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/yourusername/RepoScribe.git
   ```

2. **Build the Project:**
   ```bash
   cd RepoScribe
   dotnet build
   ```

3. **Navigate to CLI:**
   ```bash
   cd RepoScribe.CLI
   ```

## Usage

### Flatten a Codebase

```bash
repo-scribe flatten --input <path_to_codebase> --output <output_markdown_file> [--compress]
```

**Options:**
- `--input`, `-i`: The root directory to process (required).
- `--output`, `-o`: The output Markdown file. Defaults to `<input_basename>.md`.
- `--compress`, `-c`: Enable content compression.

**Example:**
```bash
repo-scribe flatten -i ./MyProject -o ./MyProjectDocumentation.md -c
```

### Manage Ignored Paths

#### Add a Path to Ignore

```bash
repo-scribe ignore add <path>
```

**Example:**
```bash
repo-scribe ignore add node_modules
```

#### List Ignored Paths

```bash
repo-scribe ignore list
```

#### Remove a Path from Ignore

```bash
repo-scribe ignore remove <path>
```

**Example:**
```bash
repo-scribe ignore remove node_modules
```

### Manage Repositories

#### Add a Repository

```bash
repo-scribe repo add <repository_url>
```

**Example:**
```bash
repo-scribe repo add https://github.com/yourusername/another-repo.git
```

#### List Repositories

```bash
repo-scribe repo list
```

#### Remove a Repository

```bash
repo-scribe repo remove <repository_url>
```

**Example:**
```bash
repo-scribe repo remove https://github.com/yourusername/another-repo.git
```

#### Clone a Repository

```bash
repo-scribe repo clone <repository_url> [--path <local_path>]
```

**Example:**
```bash
repo-scribe repo clone https://github.com/yourusername/another-repo.git -p ./ClonedRepos/another-repo
```

### Extract Code Blocks

```bash
repo-scribe extract --config <config_file>
```

**Options:**
- `--config`, `-c`: Path to the configuration file. Defaults to `config.json`.

**Example:**
```bash
repo-scribe extract -c ./config.json
```

## Configuration

### `appsettings.json`

Defines allowed file types and ignored paths.

```json
{
  "AllowedFiles": {
    ".cs": "csharp",
    ".js": "javascript",
    // ... other file types
  },
  "Ignored": {
    "node_modules": "node_modules",
    "bin": "bin",
    // ... other ignored paths
  },
  "ExtractChunksInputDirectory": "path_to_markdown_files"
}
```

### `config.json`

Used for extract operations.

```json
{
  "ExtractChunksInputDirectory": "./MarkdownFiles",
  "FlattenAllOutputDirectory": "./FlattenedOutput"
}
```

## Logging

RepoScribe uses Serilog for logging. Logs are output to the console and saved to `logs/log.txt` with daily rolling.

## Testing

Run unit tests using:

```bash
cd RepoScribe.Tests
dotnet test
```

## Contributing

Contributions are welcome! Please submit a pull request or open an issue for any enhancements or bugs.

## License

[MIT](LICENSE)
