# Coding convention
### Bracketing

```csharp
public void Test(BookCategory cat) {
	if (Enum.IsDefined(typeof(BookCategory), cat)) {
		...
	}
}

```

### Line breaking
* Max **80 chars** width before doing line-break

### Indentation
* Uses `tabs` with 4 spaces indenting
* Equals `insert spaces` for each tab character
* Always indent 1 level for a new function level (as stated above)

# Git
1. **Always** use *git-flow* workflow
1. **Always** use ensure you are working in correct branch **before** commiting changes
1. **Name** your subtasks as features with **featurenumber** and **name**, like:
    SD20G07-28 Ground` *subtask* will have branchname **feature/28-ground**


*If you have any other suggestions, please add them to this document and commit in* ***master*** *-branch.*
