def plusOne(digits: 'list[int]') -> 'list[int]':
    num = 0
    for a in range(len(digits)):
        num = num * 10 + digits[a]
    num = num + 1

    return list(map(int, str(num)))

s = plusOne([1,2,3,4,5])
print(s)


