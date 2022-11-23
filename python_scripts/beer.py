
def beers(money, price, bottleWeight, capWeight):
    sum = int(money//price)
    bottles = sum
    caps = sum
    round = 1
    while (bottles >= bottleWeight or caps >= capWeight):
        print("[round " + str(round) + "] begin beers " + str(sum) + " bottles " + str(bottles) + " caps " + str(caps))
        newBearFromBottles = 0
        newBearFromCaps = 0
        if bottles >= bottleWeight:
            newBearFromBottles = bottles//bottleWeight
            bottles = bottles%bottleWeight
        if caps >= capWeight:
            newBearFromCaps = caps // capWeight
            caps = caps % capWeight
        new_beers = newBearFromBottles + newBearFromCaps
        sum = sum + new_beers
        bottles = bottles + new_beers
        caps = caps + new_beers
        print("[round " + str(round) + "] end beers " + str(sum) + " bottles " + str(bottles) + " caps " + str(caps))        
        round = round +1
    return sum

        
if __name__ == "__main__":
    sum = beers(7,2,2,4)
    print("buy " + str(sum) + " beers")