using FluentAssertions;
using NUnit.Framework;

namespace FirstOneTo.Authentication.Service.Tests
{
    [TestFixture]
    public class PasswordAdvisorTests
    {
        [Test]
        public void blank_password_identified_as_blank()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("");
            result.Should().Be(PasswordScore.Blank);
        }

        [Test]
        public void password_has_space()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaa aaaa");
            result.Should().NotBe(PasswordScore.Blank);
            result.Should().NotBe(PasswordScore.VeryWeak);
        }

        [Test]
        public void password_has_spaces()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaa aaaa aaaaa");
            result.Should().NotBe(PasswordScore.Blank);
            result.Should().NotBe(PasswordScore.VeryWeak);
            result.Should().NotBe(PasswordScore.Weak);
        }

        [Test]
        public void
            password_with_eight_characters_and_at_least_one_capital_and_one_number_and_one_special_character_identified_as_strong
            ()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("!a1aaAaa");
            result.Should().Be(PasswordScore.VeryStrong);
        }

        [Test]
        public void password_with_eight_characters_and_at_least_one_capital_and_one_number_identified_as_strong()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aa1aaAaa");
            result.Should().Be(PasswordScore.Strong);
        }

        [Test]
        public void password_with_eight_characters_and_at_least_one_capital_identified_as_medium()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaaaAaa");
            result.Should().Be(PasswordScore.Medium);
        }

        [Test]
        public void
            password_with_eight_characters_and_at_least_one_upper_case_and_one_lower_case_is_and_at_least_one_number_and_at_least_one_special_character_is_very_strong
            ()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aa1aa!aA");
            result.Should().Be(PasswordScore.VeryStrong);
        }

        [Test]
        public void
            password_with_eight_characters_and_at_least_one_upper_case_and_one_lower_case_is_and_at_least_one_number_is_strong
            ()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aa1aaaaA");
            result.Should().Be(PasswordScore.Strong);
        }

        [Test]
        public void password_with_eight_characters_and_at_least_one_upper_case_and_one_lower_case_is_medium()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaaaaaA");
            result.Should().Be(PasswordScore.Medium);
        }

        [Test]
        public void password_with_eight_characters_and_one_is_a_number_is_medium()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaaaaa7");
            result.Should().Be(PasswordScore.Medium);
        }

        [Test]
        public void password_with_eight_letters_identified_as_weak()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaaaaaa");
            result.Should().Be(PasswordScore.Weak);
        }

        [Test]
        public void password_with_eleven_letters_identified_as_weak()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaaaaaaaaa");
            result.Should().Be(PasswordScore.Weak);
        }

        [Test]
        public void
            password_with_five_characters_with_a_capital_and_a_number_and_a_special_character_is_identified_as_strong()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("a1aA!");
            result.Should().Be(PasswordScore.Strong);
        }

        [Test]
        public void password_with_five_characters_with_a_capital_and_a_number_is_identified_as_medium()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("a1aAa");
            result.Should().Be(PasswordScore.Medium);
        }

        [Test]
        public void password_with_five_characters_with_a_capital_is_identified_as_weak()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaAa");
            result.Should().Be(PasswordScore.Weak);
        }

        [Test]
        public void password_with_five_letters_identified_as_very_weak()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaaa");
            result.Should().Be(PasswordScore.VeryWeak);
        }

        [Test]
        public void password_with_four_letters_identified_as_very_weak()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaa");
            result.Should().Be(PasswordScore.VeryWeak);
        }

        [Test]
        public void password_with_nine_letters_identified_as_weak()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaaaaaaa");
            result.Should().Be(PasswordScore.Weak);
        }

        [Test]
        public void password_with_one_letters_identified_as_very_weak()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("a");
            result.Should().Be(PasswordScore.VeryWeak);
        }

        [Test]
        public void
            password_with_seven_character_and_at_least_one_capital_and_one_number_and_one_special_character_identified_as_strong
            ()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("a1aAa!a");
            result.Should().Be(PasswordScore.Strong);
        }

        [Test]
        public void password_with_seven_character_and_at_least_one_capital_and_one_number_identified_as_medium()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("a1aAaaa");
            result.Should().Be(PasswordScore.Medium);
        }

        [Test]
        public void password_with_seven_character_and_at_least_one_capital_identified_as_weak()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaAaaa");
            result.Should().Be(PasswordScore.Weak);
        }

        [Test]
        public void password_with_seven_letters_identified_as_very_weak()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaaaaa");
            result.Should().Be(PasswordScore.VeryWeak);
        }

        [Test]
        public void
            password_with_six_letters_and_at_least_one_capital_and_one_number_and_a_special_character_identified_as_strong
            ()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("!aAa1a");
            result.Should().Be(PasswordScore.Strong);
        }

        [Test]
        public void password_with_six_letters_and_at_least_one_capital_and_one_number_identified_as_medium()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaAa1a");
            result.Should().Be(PasswordScore.Medium);
        }

        [Test]
        public void password_with_six_letters_and_at_least_one_capital_identified_as_weak()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaAaaa");
            result.Should().Be(PasswordScore.Weak);
        }

        [Test]
        public void password_with_six_letters_identified_as_very_weak()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaaaa");
            result.Should().Be(PasswordScore.VeryWeak);
        }

        [Test]
        public void password_with_ten_letters_identified_as_weak()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaaaaaaaa");
            result.Should().Be(PasswordScore.Weak);
        }

        [Test]
        public void password_with_three_letters_identified_as_very_weak()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaa");
            result.Should().Be(PasswordScore.VeryWeak);
        }

        [Test]
        public void
            password_with_twelve_characters_and_at_least_one_upper_case_and_one_lower_case_and_at_least_one_number_and_a_special_character_is_very_strong
            ()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("a1aa!aaaaaaA");
            result.Should().Be(PasswordScore.VeryStrong);
        }

        [Test]
        public void
            password_with_twelve_characters_and_at_least_one_upper_case_and_one_lower_case_and_at_least_one_number_is_very_strong
            ()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("a1aaaaaaaaaA");
            result.Should().Be(PasswordScore.VeryStrong);
        }

        [Test]
        public void password_with_twelve_characters_and_at_least_one_upper_case_and_one_lower_case_is_strong()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaaaaaaaaaA");
            result.Should().Be(PasswordScore.Strong);
        }

        [Test]
        public void password_with_twelve_characters_and_one_is_a_number_is_strong()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaaaaaaaaa1");
            result.Should().Be(PasswordScore.Strong);
        }

        [Test]
        public void password_with_twelve_letters_identified_as_medium()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aaaaaaaaaaaa");
            result.Should().Be(PasswordScore.Medium);
        }

        [Test]
        public void password_with_two_letters_identified_as_very_weak()
        {
            PasswordScore result = PasswordAdvisor.CheckStrength("aa");
            result.Should().Be(PasswordScore.VeryWeak);
        }
    }
}