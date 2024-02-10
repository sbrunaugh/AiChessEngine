def count_lines(filename):
    with open(filename, 'r') as file:
        lines = file.readlines()
    return len(lines)

# Usage
w_filename = "../train_data_white.txt"
b_filename = "../train_data_black.txt"
print(f"The file '{w_filename}' has {count_lines(w_filename)} lines.")
print(f"The file '{b_filename}' has {count_lines(b_filename)} lines.")
