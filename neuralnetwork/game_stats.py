def safe_string_to_number(s):
    try:
        return float(s)  # Convert to float (can handle integers too)
    except ValueError:
        return 0  # Return zero if parsing fails

# 0-500, 500-1000, 1000-1500, 1500-2000, 2000+
counts = [0, 0, 0, 0, 0, 0]

with open('../all_with_filtered_anotations_since1998.txt', 'r') as file:
    for line in file:
        if(line[0] == '#'):
            continue

        parts = line.split(' ')
        white_elo = safe_string_to_number(parts[3])
        black_elo = safe_string_to_number(parts[4])
        
        if(white_elo <=500):
            counts[0] += 1
        elif(white_elo > 500 and white_elo <= 1000):
            counts[1] += 1
        elif(white_elo > 1000 and white_elo <= 1500):
            counts[2] += 1
        elif(white_elo > 1500 and white_elo <= 2000):
            counts[3] += 1
        elif(white_elo > 2000 and white_elo <= 2500):
            counts[4] += 1
        else:
            counts[5] += 1

        if(black_elo <=500):
            counts[0] += 1
        elif(black_elo > 500 and black_elo <= 1000):
            counts[1] += 1
        elif(black_elo > 1000 and black_elo <= 1500):
            counts[2] += 1
        elif(black_elo > 1500 and black_elo <= 2000):
            counts[3] += 1
        elif(black_elo > 2000 and black_elo <= 2500):
            counts[4] += 1
        else:
            counts[5] += 1

        msg = "0-5: %s, 5-10: %s, 10-15: %s, 15-20: %s, 20-25: %s, 25+: %s" % (counts[0], counts[1], counts[2], counts[3], counts[4], counts[5])
        print(msg)  # Process each line as needed

#1002 2006.11.05 1-0 2792 2577 77 date_false result_false welo_false belo_false edate_false setup_false fen_false result2_false oyrange_false blen_false ###