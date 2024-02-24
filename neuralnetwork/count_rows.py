def count_lines(filename):
    with open(filename, 'r') as file:
        lines = file.readlines()
    return len(lines)

# Usage
filename = "../train_data.txt"
print(f"The file '{filename}' has {count_lines(filename)} lines.")