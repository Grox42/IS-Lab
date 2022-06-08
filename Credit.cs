namespace CreditNamespace;

public enum Purpose
{
    RefinancingLarge,
    Vehicle_realty,
    Mortgage,
    RefinancingAverage,
    LongTerm_services,
    RefinancingSmall,
    Appliances_furniture,
    NotSpecified
}

public enum Guarantee
{
    Pledge_vehicle_realty,
    Insurance,
    Deposit,
    Surety,
    NotSpecified
}

public enum Schema
{
    Annuity,
    Differentiated
}

public class Credit
{
    public static Purpose[] purposes = {
            Purpose.RefinancingLarge,
            Purpose.Vehicle_realty,
            Purpose.Mortgage,
            Purpose.RefinancingAverage,
            Purpose.LongTerm_services,
            Purpose.RefinancingSmall,
            Purpose.Appliances_furniture,
            Purpose.NotSpecified
        };

    public Purpose purpose { get; set; }
    public Guarantee[] guarantees { get; set; }
    public Schema schema { get; set; }
    public uint amount { get; set; }
    public float procent { get; set; }
    public uint term { get; set; }
    public float payment { get; set; }
    public float paymentMax { get; set; }
    public float paymentMin { get; set; }
    public float overpayment { get; set; }    

    public Credit()
    {
        Random rand = new Random();
        purpose = purposes[rand.Next(8)];
        schema = rand.Next(2) == 0 ? Schema.Annuity : Schema.Differentiated;

        if (purpose == Purpose.RefinancingLarge || purpose == Purpose.Vehicle_realty) {
            guarantees = new Guarantee[] {
                Guarantee.Pledge_vehicle_realty,
                Guarantee.Insurance
            };
            amount = (uint) rand.Next(10, 51) * 200000;
            procent = 0.12f + (float) rand.Next(20, 61) / 1000;
            term = (uint) (30 * 12 * (0.75f * amount / 10000000 + 0.25f * procent / 0.18f));
        }
        else if (purpose == Purpose.Mortgage) {
            guarantees = new Guarantee[] {
                Guarantee.Pledge_vehicle_realty,
                Guarantee.Insurance,
                Guarantee.Deposit
            };
            amount = (uint) rand.Next(10, 51) * 200000;
            procent = 0.06f + (float) rand.Next(10, 31) / 1000;
            term = (uint) (30 * 12 * (0.75f * amount / 10000000 + 0.25f * procent / 0.09f));
        }
        else if (purpose == Purpose.RefinancingAverage || purpose == Purpose.LongTerm_services) {
            guarantees = new Guarantee[] {
                Guarantee.Pledge_vehicle_realty,
                Guarantee.Insurance,
                Guarantee.Deposit,
                Guarantee.Surety
            };
            amount = (uint) rand.Next(4, 40) * 50000;
            procent = 0.12f + (float) rand.Next(30, 91) / 1000;
            term = (uint) (5 * 12 * (0.75f * amount / 1950000 + 0.25f * procent / 0.21f));
        }
        else if (purpose == Purpose.RefinancingSmall || purpose == Purpose.Appliances_furniture) {
            guarantees = new Guarantee[] {
                Guarantee.Pledge_vehicle_realty,
                Guarantee.Insurance,
                Guarantee.Deposit,
                Guarantee.Surety
            };
            amount = (uint) rand.Next(1, 20) * 10000;
            procent = 0.12f + (float) rand.Next(180, 271) / 1000;
            term = (uint) (12 * (0.75f * amount / 190000 + 0.25f * procent / 0.39f));
        }
        else {
            guarantees = new Guarantee[] { Guarantee.NotSpecified };
            amount = (uint) rand.Next(1, 10) * 5000;
            procent = 0.12f + (float) rand.Next(540, 811) / 1000;
            term = (uint) (3 * (amount / 45000 + procent / 0.93f) / 2);
        }

        if (schema == Schema.Annuity) {
            payment = amount * (float) (procent / 12 + (procent / 12) / (Math.Pow(procent / 12 + 1, term) - 1));
            overpayment = payment * term - amount;
        }
        else {
            paymentMax = amount / term + amount * procent / 12;
            float temp = amount;
            for (int i = 0; i < term; i++) {
                payment = amount / term + temp * procent / 12;
                temp -= amount / term;
                overpayment += payment;
            }
            paymentMin = payment;
            overpayment -= amount;
        }
    }

    public Credit(string parametre)
    {
        guarantees = new Guarantee[0];

        if (parametre == "req") {
            Console.WriteLine(
                "Здравствуйте, вы используете экспертную систему для выбора наиболее выгодного кредитa.\n" +
                "Приступим к выбору.\n"
                );

            DefinePurpose();
            DefineGuarantees();
            DefineSchema();
            if (amount == 0)
                DefineAmount();
            
            Additionally();
            PrintAll();

            while (Change());
            PrintAll();

            Console.WriteLine("Создание модели завершено.\n");
        }
    }

    public void DefinePurpose()
    {
        bool flag;
        string? str;

        Console.WriteLine(
            "Для какой цели вам понадобился кредит? (введите соответствующую цифру)\n" +
            "1 - Для покупки недвижимости или транспорта.\n" +
            "2 - Для покупки жилья. (Ипотека)\n" +
            "3 - Для рефинансирования.\n" +
            "4 - Для оплаты долгосрочных услуг. (Ремонт, обучение, отдых за границей)\n" +
            "5 - Для покупки мебели и электротехники.\n" +
            "6 - Нет желания указывать причину.\n" +
            "7 - Подробнее об ипотеке.\n" +
            "8 - Подробнее о рефинансирование.\n"
            );
        
        do {
            flag = false;
            Console.Write("Ввод: ");
            str = Console.ReadLine();

            if (str == null || str.Length != 1) {
                Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                flag = true;
            }
            else {
                switch (str[0]) {
                    case '1':
                        purpose = Purpose.Vehicle_realty;
                        break;
                    case '2':
                        purpose = Purpose.Mortgage;
                        break;
                    case '3':
                        if (amount == 0) {
                            Console.WriteLine();
                            DefineAmount();
                        }
                        if (amount >= 2000000)
                            purpose = Purpose.RefinancingLarge;
                        else if (amount >= 200000)
                            purpose = Purpose.RefinancingAverage;
                        else
                            purpose = Purpose.RefinancingSmall;
                        break;
                    case '4':
                        purpose = Purpose.LongTerm_services;
                        break;
                    case '5':
                        purpose = Purpose.Appliances_furniture;
                        break;
                    case '6':
                        purpose = Purpose.NotSpecified;
                        break;
                    case '7':
                        Console.WriteLine(
                            "Ипотека - вариант залога недвижимости, при котором объект недвижимости\n" +
                            "остаётся во владении и пользовании должника, а кредитор, в случае невыполнения\n" +
                            "должником своего обязательства, приобретает право получить удовлетворение за счёт\n" +
                            "реализации данного имущества.\n"
                            );
                        flag = true;
                        break;
                    case '8':
                        Console.WriteLine(
                            "Рефинансирование - замена существующего долгового обязательства\n" +
                            "на новое долговое обязательство на рыночных условиях.\n"
                            );
                        flag = true;
                        break;
                    default:
                        Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                        flag = true;
                        break;
                }
            }
        } while (flag);

        Console.WriteLine();
    }

    public void DefineGuarantees()
    {
        bool flag;
        string? str;

        Console.WriteLine(
            "Как вы можете гарантировать выполнение своих долговых обязательств? (введите соответствующие цифры без пробелов)\n" +
            "Если вы не можете гарантировать выполнение своих долговых обязательств, то нажмите Enter, не вводя цифры.\n" +
            "1 - Залог недвижимости или транспорта.\n" +
            "2 - Кредитное страхование.\n" +
            "3 - Депозит.\n" +
            "4 - Поручительство.\n" +
            "5 - Подробнее о депозите.\n"
            );
        
        int size;
        int[] check = new int[4];

        do {
            flag = false;
            Console.Write("Ввод: ");
            str = Console.ReadLine();

            if (str == null || str.Length == 0) {
                guarantees = new Guarantee[1];
                guarantees[0] = Guarantee.NotSpecified;
            }
            else {
                size = 0;
                for (int i = 0; i < 4; i++)
                    check[i] = 0;
                
                foreach (char elem in str)
                    if (elem == '1' || elem == '2' || elem == '3' || elem == '4')
                        check[(int) elem - 49]++;
                    else if (elem == '5') {
                        Console.WriteLine(
                            "Депозит — финансовый термин, включающий в себя как банковские вклады физических лиц,\n" +
                            "так и передачу других видов ценностей от юридических лиц в банк или депозитарий.\n" +
                            "Передача ценных бумаг, драгоценных металлов, предметов искусства\n" +
                            "и других ценностей может называться только депозитом.\n"
                            );

                        flag = true;
                        break;
                    }
                    else {
                        Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                        flag = true;
                        break;
                    }
                
                foreach (int quantity in check)
                    if (quantity > 1) {
                        Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.");
                        flag = true;
                        break;
                    }
                    else if (quantity == 1)
                        size++;
                
                if (!flag) {
                    guarantees = new Guarantee[size];

                    for (int i = 0, k = 0; i < 4; i++) {
                        if (check[i] == 1) 
                            switch (i) {
                                case 0:
                                    guarantees[k++] = Guarantee.Pledge_vehicle_realty;
                                    break;
                                case 1:
                                    guarantees[k++] = Guarantee.Insurance;
                                    break;
                                case 2:
                                    guarantees[k++] = Guarantee.Deposit;
                                    break;
                                case 3:
                                    guarantees[k++] = Guarantee.Surety;
                                    break;
                            }
                    }
                }
            }
        } while (flag);

        Console.WriteLine();
    }

    public void DefineSchema()
    {
        bool flag;
        string? str;

        Console.WriteLine(
            "Какую схему погашения задолжности вы предпочитаете? (введите цифру)\n" +
            "1 - Аннуитетная схема.\n" +
            "2 - Дифференцированная схема.\n" +
            "3 - Подробнее о аннуитетной схеме.\n" +
            "4 - Подробнее о дифференцированной схеме.\n"
            );

        do {
            flag = false;
            Console.Write("Ввод: ");
            str = Console.ReadLine();

            if (str == null || str.Length != 1) {
                Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                flag = true;
            }
            else {
                switch (str[0]) {
                    case '1':
                        schema = Schema.Annuity;
                        if (paymentMax != 0) {
                            Console.WriteLine();
                            DefinePayment();
                        }
                        break;
                    case '2':
                        schema = Schema.Differentiated;
                        if (payment != 0) {
                            Console.WriteLine();
                            DefinePaymentMax();
                        }
                        break;
                    case '3':
                        Console.WriteLine(
                            "Аннуитетная схема подразумевает фиксированный ежемесячный платеж.\n" +
                            "Такая схема отличается большей переплатой по кредиту.\n"
                            );
                        flag = true;
                        break;
                    case '4':
                        Console.WriteLine(
                            "Дифференцированная схема подразумевает последовательно уменьшение суммы ежемесячных платежей.\n" +
                            "Такая схема отличается меньшей переплатой по кредиту.\n"
                            );
                        flag = true;
                        break;
                    default:
                        Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                        flag = true;
                        break;
                }
            }
        } while(flag);

        if (payment == 0 && paymentMax == 0)
            Console.WriteLine();
    }

    public void DefineAmount()
    {
        bool flag;
        string? str;

        Console.WriteLine("На какую суммы вы хотите взять кредит? (введите целое число)\n");

        do {
            flag = false;
            Console.Write("Ввод: ");
            str = Console.ReadLine();
            
            if (str == null || str.Length == 0) {
                Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                flag = true;
            }
            else {
                foreach (char elem in str)
                    if ((int) elem < 48 && (int) elem > 57) {
                        Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                        flag = true;
                        break;
                    }
                if (!flag)
                    amount = (uint) Convert.ToInt32(str);
            }
        } while (flag);

        Console.WriteLine();
    }

    public void DefineProcent()
    {
        bool flag;
        string? str;

        Console.WriteLine(
            "Под какой процент годовых вы хотите взять кредит?\n" +
            "Введите десятичную дробь без разряда единиц и знака-разделителя между целой и дробной частями. (0,025 => 025)\n"
            );

        do {
            flag = false;
            Console.Write("Ввод: ");
            str = Console.ReadLine();
            
            if (str == null || str.Length == 0) {
                Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                flag = true;
            }
            else {
                foreach (char ch in str)
                    if ((int) ch < 48 && (int) ch > 57) {
                        Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                        flag = true;
                        break;
                    }

                if (!flag)
                    procent = (float) Convert.ToDouble("0," + str);
            }
        } while (flag);

        Console.WriteLine();
    }

    public void DefineTerm()
    {
        bool flag;
        string? str;
        
        Console.WriteLine("На какое количество месяцев вы хотите взять кредит? (Введите целое число)\n");

        do {
            flag = false;
            Console.Write("Ввод: ");
            str = Console.ReadLine();

            if (str == null || str.Length == 0) {
                Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                flag = true;
            }
            else {
                foreach (char ch in str)
                    if ((int) ch < 48 && (int) ch > 57) {
                        Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                        flag = true;
                        break;
                    }
                if (!flag)
                    term = (uint) Convert.ToInt32(str);
            }
        } while (flag);

        Console.WriteLine();
    }

    public void DefinePayment()
    {
        bool flag;
        string? str;

        Console.WriteLine("Какую сумму вы готовы платить ежемесячно для погашения долга? (Введите целое число)\n");

        do {
            flag = false;
            Console.Write("Ввод: ");
            str = Console.ReadLine();

            if (str == null || str.Length == 0) {
                Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                flag = true;
            }
            else {
                foreach (char ch in str)
                    if ((int) ch < 48 && (int) ch > 57) {
                        Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                        flag = true;
                        break;
                    }
                if (!flag)
                    payment = (float) Convert.ToInt32(str);
            }
        } while (flag);

        Console.WriteLine();
    }

    public void DefinePaymentMax()
    {
        bool flag;
        string? str;

        Console.WriteLine("Какую сумму вы готовы заплатить при первом платеже? (Введите целое число)\n");

        do {
            flag = false;
            Console.Write("Ввод: ");
            str = Console.ReadLine();

            if (str == null || str.Length == 0) {
                Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                flag = true;
            }
            else {
                foreach (char ch in str)
                    if ((int) ch < 48 && (int) ch > 57) {
                        Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                        flag = true;
                        break;
                    }
                if (!flag)
                    paymentMax = (float) Convert.ToInt32(str);
            }
        } while (flag);

        Console.WriteLine();
    }

    public void DefineOverpayment()
    {
        bool flag;
        string? str;

        Console.WriteLine("Какую сумму вы готовы переплатить по процентам банку? (Введите целое число)\n");

        do {
            flag = false;
            Console.Write("Ввод: ");
            str = Console.ReadLine();

            if (str == null || str.Length == 0) {
                Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                flag = true;
            }
            else {
                foreach (char elem in str)
                    if ((int) elem < 48 && (int) elem > 57) {
                        Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                        flag = true;
                        break;
                    }
                
                if (!flag)
                    overpayment = (float) Convert.ToInt32(str);
            }
        } while (flag);

        Console.WriteLine();
    }

    public void Additionally()
    {
        bool flag;
        string? str;

        Console.WriteLine(
            "Желаете указать дополнительные параметры? (введите соответствующие цифры без пробелов)\n" +
            "Если вы не желаете указать дополнительные параметры, то нажмите Enter, не вводя цифры.\n" +
            "1 - Указать процент годовых.\n" +
            "2 - Указать срок погашения задолжности.\n" +
            "3 - Укзать ежемесячные (аннуитетная схема) или первый (дифференциальная схема) платеж.\n" +
            "4 - Указать переплату по кредиту.\n"
            );
        
        int[] check = new int[4];

        do {
            flag = false;
            Console.Write("Ввод: ");
            str = Console.ReadLine();

            if (str != null && str.Length != 0) {
                for (int i = 0; i < 4; i++)
                    check[i] = 0;

                foreach (char elem in str)
                    if (elem == '1' || elem == '2' || elem == '3' || elem == '4')
                        check[(int) elem - 49]++;
                    else {
                        Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                        flag = true;
                        break;
                    }

                foreach (int quantity in check)
                    if (quantity > 1) {
                        Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.");
                        flag = true;
                        break;
                    }

                if (!flag) {
                    Console.WriteLine();
                    for (int i = 0; i < 4; i++)
                        if (check[i] == 1)
                            switch (i) {
                                case 0:
                                    DefineProcent();
                                    break;
                                case 1:
                                    DefineTerm();
                                    break;
                                case 2:
                                    if (schema == Schema.Annuity)
                                        DefinePayment();
                                    else
                                        DefinePaymentMax();
                                    break;
                                case 3:
                                    DefineOverpayment();
                                    break;
                            }
                }
            }
        } while (flag);

        if (procent + term + payment + paymentMax + overpayment == 0)
            Console.WriteLine();
    }

    public bool Change()
    {
        bool flag;
        string? str;
        int num = 4;
        int[] temp = new int[4];

        Console.WriteLine(
            "Желаете изменить параметры? (введите соответствующую цифру)\n" +
            "Если вы не желаете изменять параметры, то нажмите Enter, не вводя цифру.\n" +
            "1 - Если хотите изменить цель кредита.\n" +
            "2 - Если хотете изменить список ваших гарантий.\n" +
            "3 - Если хотите именить схему возврата долга.\n" +
            "4 - Если хотите именить сумму кредита."
            );
        
        if (procent != 0) {
            Console.WriteLine($"{++num} - Если хотите изменить процентную ставку кредита.");
            temp[0]++;
        }
        if (term != 0) {
            Console.WriteLine($"{++num} - Если хотите изменить срок уплаты задолжности.");
            temp[1]++;
        }
        if (schema == Schema.Annuity && payment != 0) {
            Console.WriteLine($"{++num} - Если хотите изменить ежемесячную выплату.");
            temp[2]++;
        }
        else if (schema == Schema.Differentiated && paymentMax != 0) {
            Console.WriteLine($"{++num} - Если хотите изменить размер первой выплаты.");
            temp[2]++;
        }
        if (overpayment != 0) {
            Console.WriteLine($"{++num} - Если хоите изменить размер переплаты по кредиту.");
            temp[3]++;
        }
        
        Console.WriteLine();

        do {
            flag = false;
            Console.Write("Ввод: ");
            str = Console.ReadLine();

            if (str == null || str.Length == 0) {
                Console.WriteLine();
                return false;
            }
            if (str.Length != 1 || str[0] < 49 || str[0] > 48 + num) {
                Console.WriteLine("Вы ввели некорректное значение, попробуйте еще раз.\n");
                flag = true;
            }
            else {
                Console.WriteLine();
                switch (str[0]) {
                    case '1':
                        DefinePurpose();
                        break;
                    case '2':
                        DefineGuarantees();
                        break;
                    case '3':
                        DefineSchema();
                        break;
                    case '4':
                        DefineAmount();
                        break;
                }

                num = Convert.ToInt32(str) - 4;
                for (int i = 0; num > 0; i++) {
                    num -= temp[i];
                    if (num == 0)
                        switch (i) {
                            case 0:
                                DefineProcent();
                                break;
                            case 1:
                                DefineTerm();
                                break;
                            case 2:
                                if (schema == Schema.Annuity)
                                    DefinePayment();
                                else
                                    DefinePaymentMax();
                                break;
                            case 3:
                                DefineOverpayment();
                                break;
                        }
                }
            }
        } while (flag);

        return true;
    }

    public Credit[] Search(Credit[] dataBase)
    {
        int size = 3, counter;
        float ratioNew;
        float[] ratioNow = new float[size];
        Credit[] result = new Credit[size];
        bool flag;

        foreach (Credit credit in dataBase)
            if (purpose == credit.purpose && schema == credit.schema) {
                flag = false;
                foreach (var elem1 in guarantees)
                    foreach (var elem2 in credit.guarantees)
                        if (elem1 == elem2)
                            flag = true;
                if (flag) {
                    if (size > 0) {
                        result[--size] = credit.Clone();

                        counter = 1;
                        ratioNow[size] += Math.Abs(credit.amount / (float) amount - 1);
                        if (procent != 0) {
                            ratioNow[size] += Math.Abs(credit.procent / procent - 1);
                            counter++;
                        }
                        if (term != 0) {
                            ratioNow[size] += Math.Abs(credit.term / (float) term - 1);
                            counter++;
                        }
                        if (schema == Schema.Annuity && payment != 0) {
                            ratioNow[size] += Math.Abs(credit.payment / payment - 1);
                            counter++;
                        }
                        if (schema == Schema.Differentiated && paymentMax != 0) {
                            ratioNow[size] += Math.Abs(credit.paymentMax / paymentMax - 1);
                            counter++;
                        }
                        if (overpayment != 0) {
                            ratioNow[size] += Math.Abs(credit.overpayment / overpayment - 1);
                            counter++;
                        }
                        ratioNow[size] /= counter;

                        if (size == 0) {
                            float temp;
                            for (int i = 0; i < ratioNow.Length - 1; i++)
                                for (int j = i + 1; j < ratioNow.Length; j++)
                                    if (ratioNow[i] > ratioNow[j]) {
                                        temp = ratioNow[i];
                                        ratioNow[i] = ratioNow[j];
                                        ratioNow[j] = temp;
                                    }
                        }
                    }
                    else {
                        counter = 1;
                        ratioNew = 0;
                        ratioNew += Math.Abs(credit.amount / (float) amount - 1);
                        if (procent != 0) {
                            ratioNew += Math.Abs(credit.procent / procent - 1);
                            counter++;
                        }
                        else if (credit.procent < procent)
                            counter++;
                        if (term != 0) {
                            ratioNew += Math.Abs(credit.term / (float) term - 1);
                            counter++;
                        }
                        else if (credit.term < term)
                            counter++;
                        if (schema == Schema.Annuity)
                            if (payment != 0) {
                                ratioNew += Math.Abs(credit.payment / payment - 1);
                                counter++;
                            }
                            else if (credit.payment < payment)
                                counter++;
                        else if (schema == Schema.Differentiated)
                            if (paymentMax != 0) {
                                ratioNew += Math.Abs(credit.paymentMax / paymentMax - 1);
                                counter++;
                            }
                            else if (credit.paymentMax < paymentMax)
                                counter++;
                        if (overpayment != 0) {
                            ratioNew += Math.Abs(credit.overpayment / overpayment - 1);
                            counter++;
                        }
                        else if (credit.overpayment < overpayment)
                            counter++;
                        ratioNew /= counter;

                        if (ratioNew < ratioNow[0] || (ratioNew == ratioNow[0] && credit.overpayment < result[0].overpayment)) {
                            for (int i = result.Length - 1; i > 0; i--) {
                                ratioNow[i] = ratioNow[i - 1];
                                result[i] = result[i - 1];
                            }
                            ratioNow[0] = ratioNew;
                            result[0] = credit.Clone();
                        }
                    }
                }
            }
        
        return result;
    }

    public Credit Clone()
    {
        Credit temp = new Credit();
        temp.purpose = purpose;
        temp.guarantees = new Guarantee[guarantees.Length];
        for (int i = 0; i < temp.guarantees.Length; i++)
            temp.guarantees[i] = guarantees[i];
        temp.schema = schema;
        temp.amount = amount;
        temp.procent = procent;
        temp.term = term;
        temp.payment = payment;
        temp.paymentMax = paymentMax;
        temp.paymentMin = paymentMin;
        temp.overpayment = overpayment;
        return temp;
    }

    public void PrintAll()
    {
        Console.WriteLine($"Purpose: {purpose}");
        Console.Write("Guarantee: ");
        foreach (Guarantee elem in guarantees)
            Console.Write($"{elem} ");
        Console.WriteLine();
        Console.WriteLine($"Schema: {schema}");
        Console.WriteLine($"Amount: {amount}");
        Console.WriteLine($"Procebt: {Math.Round((double) procent, 3)}");
        Console.WriteLine($"Term: {term}");
        if (schema == Schema.Annuity)
            Console.WriteLine($"Payment: {Math.Round((double) payment, 2)}");
        else {
            Console.WriteLine($"Payment first: {Math.Round((double) paymentMax, 2)}");
            Console.WriteLine($"Payment last: {Math.Round((double) paymentMin, 2)}");
        }
        Console.WriteLine($"OverPayment: {Math.Round((double) overpayment, 2)}");
        Console.WriteLine();
    }

}
