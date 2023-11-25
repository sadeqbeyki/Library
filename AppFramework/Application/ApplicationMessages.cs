namespace AppFramework.Application
{
    public class ApplicationMessages
    {
        public const string DuplicatedRecord = "امکان ثبت رکورد تکراری وجود ندارد. با نام دیگری تلاش کنید.";
        public const string RecordNotFound = "رکورد با اطلاعات فوق موجود نیست";
        public const string PasswordNotMatch = "گذرواژه ها با هم مطابقت ندارند";
        public const string WrongUserPass = "نام کاربری یا گذرواژه اشتباه است";
        public const string MoreThanStock = "مقدار درخواستی از موجودی انبار بیشتر است";
        public const string ProblemFound = "عملیات ایجاد با مشکل مواجه شد";

        //Lend System
        public const string BookIsLoaned = "کتاب امانت داده شده است";
        public const string TheBookIsNotInStock = "کتاب در انبار موجود نیست";
        public const string BookWasAlreadyReturned = "کتاب قبلا به انبار بازگردانده شده یا هنوز امانت ثبت نشده است";
        public const string ReturnFailed = "مشکل در بازپس دادن کتاب";
        public const string LendFailed = "مشکل در امانت گرفتن کتاب";
        public const string ModelIsNull = "فیلد های الزامی را پر کنید";
        public const string DuplicateLendByMember = "کاربر قبلا این کتاب را به امانت گرفته و بازگشت نداده است";
        public const string MemberDidntReturnedTheBook = "تا کنون کاربر نتوانسته است کتاب ها را به موقع به کتابخانه برگرداند ";
    }
}
