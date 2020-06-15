/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/08 10:37
	Filee: 	    CBigInteger.cs
	Author:		膜拜大神！！！！

	Purpose:	大数类
                提供大数存储和四则运算
*********************************************************************/

using System;
using System.Collections.Generic;

namespace CoffeeBean
{
    using DType = System.UInt32; // This could be UInt32, UInt16 or Byte; not UInt64.

    #region DigitsArray

    internal class DigitsArray
    {
        internal static readonly DType AllBits;

        // = ~((DType)0);
        internal static readonly DType HiBitSet;

        private DType[] m_data;

        private int m_dataUsed;

        static DigitsArray()
        {
            unchecked
            {
                AllBits = (DType)~( (DType)0 );
                HiBitSet = (DType)( ( (DType)1 ) << ( DataSizeBits ) - 1 );
            }
        }

        internal DigitsArray( int size )
        {
            Allocate( size, 0 );
        }

        internal DigitsArray( int size, int used )
        {
            Allocate( size, used );
        }

        internal DigitsArray( DType[] copyFrom )
        {
            Allocate( copyFrom.Length );
            CopyFrom( copyFrom, 0, 0, copyFrom.Length );
            ResetDataUsed();
        }

        internal DigitsArray( DigitsArray copyFrom )
        {
            Allocate( copyFrom.Count, copyFrom.DataUsed );
            Array.Copy( copyFrom.m_data, 0, m_data, 0, copyFrom.Count );
        }

        internal static int DataSizeBits
        {
            get
            {
                return sizeof( DType ) * 8;
            }
        }

        // = 0x80000000;
        internal static int DataSizeOf
        {
            get
            {
                return sizeof( DType );
            }
        }

        internal int Count
        {
            get
            {
                return m_data.Length;
            }
        }

        internal int DataUsed
        {
            get
            {
                return m_dataUsed;
            }
            set
            {
                m_dataUsed = value;
            }
        }

        internal bool IsNegative
        {
            get
            {
                return ( m_data[m_data.Length - 1] & HiBitSet ) == HiBitSet;
            }
        }

        internal bool IsZero
        {
            get
            {
                return m_dataUsed == 0 || ( m_dataUsed == 1 && m_data[0] == 0 );
            }
        }

        internal DType this[int index]
        {
            get
            {
                if ( index < m_dataUsed )
                {
                    return m_data[index];
                }

                return ( IsNegative ? (DType)AllBits : (DType)0 );
            }
            set
            {
                m_data[index] = value;
            }
        }

        public void Allocate( int size )
        {
            Allocate( size, 0 );
        }

        public void Allocate( int size, int used )
        {
            m_data = new DType[size + 1];
            m_dataUsed = used;
        }

        internal static int ShiftLeft( DType[] buffer, int shiftCount )
        {
            int shiftAmount = DigitsArray.DataSizeBits;
            int bufLen = buffer.Length;

            while ( bufLen > 1 && buffer[bufLen - 1] == 0 )
            {
                bufLen--;
            }

            for ( int count = shiftCount; count > 0; count -= shiftAmount )
            {
                if ( count < shiftAmount )
                {
                    shiftAmount = count;
                }

                ulong carry = 0;

                for ( int i = 0; i < bufLen; i++ )
                {
                    ulong val = ( ( ulong ) buffer[i] ) << shiftAmount;
                    val |= carry;

                    buffer[i] = (DType)( val & DigitsArray.AllBits );
                    carry = ( val >> DigitsArray.DataSizeBits );
                }

                if ( carry != 0 )
                {
                    if ( bufLen + 1 <= buffer.Length )
                    {
                        buffer[bufLen] = (DType)carry;
                        bufLen++;
                        carry = 0;
                    }
                    else
                    {
                        throw new OverflowException();
                    }
                }
            }

            return bufLen;
        }

        internal static int ShiftRight( DType[] buffer, int shiftCount )
        {
            int shiftAmount = DigitsArray.DataSizeBits;
            int invShift = 0;
            int bufLen = buffer.Length;

            while ( bufLen > 1 && buffer[bufLen - 1] == 0 )
            {
                bufLen--;
            }

            for ( int count = shiftCount; count > 0; count -= shiftAmount )
            {
                if ( count < shiftAmount )
                {
                    shiftAmount = count;
                    invShift = DigitsArray.DataSizeBits - shiftAmount;
                }

                ulong carry = 0;

                for ( int i = bufLen - 1; i >= 0; i-- )
                {
                    ulong val = ( ( ulong ) buffer[i] ) >> shiftAmount;
                    val |= carry;

                    carry = ( (ulong)buffer[i] ) << invShift;
                    buffer[i] = (DType)( val );
                }
            }

            while ( bufLen > 1 && buffer[bufLen - 1] == 0 )
            {
                bufLen--;
            }

            return bufLen;
        }

        internal void CopyFrom( DType[] source, int sourceOffset, int offset, int length )
        {
            Array.Copy( source, sourceOffset, m_data, 0, length );
        }

        internal void CopyTo( DType[] array, int offset, int length )
        {
            Array.Copy( m_data, 0, array, offset, length );
        }

        internal void ResetDataUsed()
        {
            m_dataUsed = m_data.Length;

            if ( IsNegative )
            {
                while ( m_dataUsed > 1 && m_data[m_dataUsed - 1] == AllBits )
                {
                    --m_dataUsed;
                }

                m_dataUsed++;
            }
            else
            {
                while ( m_dataUsed > 1 && m_data[m_dataUsed - 1] == 0 )
                {
                    --m_dataUsed;
                }

                if ( m_dataUsed == 0 )
                {
                    m_dataUsed = 1;
                }
            }
        }

        internal int ShiftLeft( int shiftCount )
        {
            return ShiftLeft( m_data, shiftCount );
        }

        internal int ShiftLeftWithoutOverflow( int shiftCount )
        {
            List<DType> temporary = new List<DType> ( m_data );
            int shiftAmount = DigitsArray.DataSizeBits;

            for ( int count = shiftCount; count > 0; count -= shiftAmount )
            {
                if ( count < shiftAmount )
                {
                    shiftAmount = count;
                }

                ulong carry = 0;

                for ( int i = 0; i < temporary.Count; i++ )
                {
                    ulong val = ( ( ulong ) temporary[i] ) << shiftAmount;
                    val |= carry;

                    temporary[i] = (DType)( val & DigitsArray.AllBits );
                    carry = ( val >> DigitsArray.DataSizeBits );
                }

                if ( carry != 0 )
                {
                    temporary.Add( 0 );
                    temporary[temporary.Count - 1] = (DType)carry;
                }
            }

            m_data = new DType[temporary.Count];
            temporary.CopyTo( m_data );
            return m_data.Length;
        }

        internal int ShiftRight( int shiftCount )
        {
            return ShiftRight( m_data, shiftCount );
        }
    }

    #endregion DigitsArray

    /// <summary>
    /// Represents a integer of abitrary length.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A BigInt object is immutable like System.String. The object can not be modifed, and new BigInt objects are
    /// created by using the operations of existing BigInt objects.
    /// </para>
    /// <para>
    /// Internally a BigInt object is an array of ? that is represents the digits of the n-place integer. Negative BigIntegers
    /// are stored internally as 1's complements, thus every BigInt object contains 1 or more padding elements to hold the sign.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// public class MainProgram
    /// {
    ///        [STAThread]
    ///        public static void Main(string[] args)
    ///        {
    ///            BigInt a = new BigInt(25);
    ///            a = a + 100;
    ///
    ///            BigInt b = new BigInt("139435810094598308945890230913");
    ///
    ///            BigInt c = b / a;
    ///            BigInt d = b % a;
    ///
    ///            BigInt e = (c * a) + d;
    ///            if (e != b)
    ///            {
    ///                Console.WriteLine("Can never be true.");
    ///            }
    ///        }
    ///    </code>
    /// </example>
    public class CBigInt
    {
        private DigitsArray m_digits;

        #region Constructors

        /// <summary>
        /// Create a BigInt with an integer value of 0.
        /// </summary>
        public CBigInt()
        {
            m_digits = new DigitsArray( 1, 1 );
        }

        /// <summary>
        /// Creates a BigInt with the value of the operand.
        /// </summary>
        /// <param name="number">A long.</param>
        public CBigInt( long number )
        {
            m_digits = new DigitsArray( ( 8 / DigitsArray.DataSizeOf ) + 1, 0 );

            while ( number != 0 && m_digits.DataUsed < m_digits.Count )
            {
                m_digits[m_digits.DataUsed] = (DType)( number & DigitsArray.AllBits );
                number >>= DigitsArray.DataSizeBits;
                m_digits.DataUsed++;
            }

            m_digits.ResetDataUsed();
        }

        /// <summary>
        /// Creates a BigInt with the value of the operand. Can never be negative.
        /// </summary>
        /// <param name="number">A unsigned long.</param>
        public CBigInt( ulong number )
        {
            m_digits = new DigitsArray( ( 8 / DigitsArray.DataSizeOf ) + 1, 0 );

            while ( number != 0 && m_digits.DataUsed < m_digits.Count )
            {
                m_digits[m_digits.DataUsed] = (DType)( number & DigitsArray.AllBits );
                number >>= DigitsArray.DataSizeBits;
                m_digits.DataUsed++;
            }

            m_digits.ResetDataUsed();
        }

        /// <summary>
        /// Creates a BigInt initialized from the byte array.
        /// </summary>
        /// <param name="array"></param>
        public CBigInt( byte[] array )
        {
            ConstructFrom( array, 0, array.Length );
        }

        /// <summary>
        /// Creates a BigInt initialized from the byte array ending at <paramref name="length" />.
        /// </summary>
        /// <param name="array">A byte array.</param>
        /// <param name="length">Int number of bytes to use.</param>
        public CBigInt( byte[] array, int length )
        {
            ConstructFrom( array, 0, length );
        }

        /// <summary>
        /// Creates a BigInt initialized from <paramref name="length" /> bytes starting at <paramref name="offset" />.
        /// </summary>
        /// <param name="array">A byte array.</param>
        /// <param name="offset">Int offset into the <paramref name="array" />.</param>
        /// <param name="length">Int number of bytes.</param>
        public CBigInt( byte[] array, int offset, int length )
        {
            ConstructFrom( array, offset, length );
        }

        /// <summary>
        /// Creates a BigInt in base-10 from the parameter.
        /// </summary>
        /// <remarks>
        ///  The new BigInt is negative if the <paramref name="digits" /> has a leading - (minus).
        /// </remarks>
        /// <param name="digits">A string</param>
        public CBigInt( string digits )
        {
            Construct( digits, 10 );
        }

        /// <summary>
        /// Creates a BigInt in base and value from the parameters.
        /// </summary>
        /// <remarks>
        ///  The new BigInt is negative if the <paramref name="digits" /> has a leading - (minus).
        /// </remarks>
        /// <param name="digits">A string</param>
        /// <param name="radix">A int between 2 and 36.</param>
        public CBigInt( string digits, int radix )
        {
            Construct( digits, radix );
        }

        /// <summary>
        /// Copy constructor, doesn't copy the digits parameter, assumes <code>this</code> owns the DigitsArray.
        /// </summary>
        /// <remarks>The <paramef name="digits" /> parameter is saved and reset.</remarks>
        /// <param name="digits"></param>
        private CBigInt( DigitsArray digits )
        {
            digits.ResetDataUsed();
            this.m_digits = digits;
        }

        private void Construct( string digits, int radix )
        {
            if ( digits == null )
            {
                throw new ArgumentNullException( "digits" );
            }

            CBigInt multiplier = new CBigInt ( 1 );
            CBigInt result = new CBigInt();
            digits = digits.ToUpper( System.Globalization.CultureInfo.CurrentCulture ).Trim();

            int nDigits = ( digits[0] == '-' ? 1 : 0 );

            for ( int idx = digits.Length - 1; idx >= nDigits; idx-- )
            {
                int d = ( int ) digits[idx];

                if ( d >= '0' && d <= '9' )
                {
                    d -= '0';
                }
                else if ( d >= 'A' && d <= 'Z' )
                {
                    d = ( d - 'A' ) + 10;
                }
                else
                {
                    throw new ArgumentOutOfRangeException( "digits" );
                }

                if ( d >= radix )
                {
                    throw new ArgumentOutOfRangeException( "digits" );
                }

                result += ( multiplier * d );
                multiplier *= radix;
            }

            if ( digits[0] == '-' )
            {
                result = -result;
            }

            this.m_digits = result.m_digits;
        }

        private void ConstructFrom( byte[] array, int offset, int length )
        {
            if ( array == null )
            {
                throw new ArgumentNullException( "array" );
            }

            if ( offset > array.Length || length > array.Length )
            {
                throw new ArgumentOutOfRangeException( "offset" );
            }

            if ( length > array.Length || ( offset + length ) > array.Length )
            {
                throw new ArgumentOutOfRangeException( "length" );
            }

            int estSize = length / 4;
            int leftOver = length & 3;

            if ( leftOver != 0 )
            {
                ++estSize;
            }

            m_digits = new DigitsArray( estSize + 1, 0 ); // alloc one extra since we can't init -'s from here.

            for ( int i = offset + length - 1, j = 0; ( i - offset ) >= 3; i -= 4, j++ )
            {
                m_digits[j] = (DType)( ( array[i - 3] << 24 ) + ( array[i - 2] << 16 ) + ( array[i - 1] << 8 ) + array[i] );
                m_digits.DataUsed++;
            }

            DType accumulator = 0;

            for ( int i = leftOver; i > 0; i-- )
            {
                DType digit = array[offset + leftOver - i];
                digit = ( digit << ( ( i - 1 ) * 8 ) );
                accumulator |= digit;
            }

            m_digits[m_digits.DataUsed] = accumulator;

            m_digits.ResetDataUsed();
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// A bool value that is true when the BigInt is negative (less than zero).
        /// </summary>
        /// <value>
        /// A bool value that is true when the BigInt is negative (less than zero).
        /// </value>
        public bool IsNegative { get { return m_digits.IsNegative; } }

        /// <summary>
        /// A bool value that is true when the BigInt is exactly zero.
        /// </summary>
        /// <value>
        /// A bool value that is true when the BigInt is exactly zero.
        /// </value>
        public bool IsZero { get { return m_digits.IsZero; } }

        #endregion Public Properties

        #region Implicit Type Operators Overloads

        /// <summary>
        /// Creates a BigInt from a long.
        /// </summary>
        /// <param name="value">A long.</param>
        /// <returns>A BigInt initialzed by <paramref name="value" />.</returns>
        public static implicit operator CBigInt( long value )
        {
            return ( new CBigInt( value ) );
        }

        /// <summary>
        /// Creates a BigInt from a ulong.
        /// </summary>
        /// <param name="value">A ulong.</param>
        /// <returns>A BigInt initialzed by <paramref name="value" />.</returns>
        public static implicit operator CBigInt( ulong value )
        {
            return ( new CBigInt( value ) );
        }

        /// <summary>
        /// Creates a BigInt from a int.
        /// </summary>
        /// <param name="value">A int.</param>
        /// <returns>A BigInt initialzed by <paramref name="value" />.</returns>
        public static implicit operator CBigInt( int value )
        {
            return ( new CBigInt( (long)value ) );
        }

        /// <summary>
        /// Creates a BigInt from a uint.
        /// </summary>
        /// <param name="value">A uint.</param>
        /// <returns>A BigInt initialzed by <paramref name="value" />.</returns>
        public static implicit operator CBigInt( uint value )
        {
            return ( new CBigInt( (ulong)value ) );
        }

        #endregion Implicit Type Operators Overloads

        #region Addition and Subtraction Operator Overloads

        /// <summary>
        /// Adds two BigIntegers and returns a new BigInt that represents the sum.
        /// </summary>
        /// <param name="leftSide">A BigInt</param>
        /// <param name="rightSide">A BigInt</param>
        /// <returns>The BigInt result of adding <paramref name="leftSide" /> and <paramref name="rightSide" />.</returns>
        public static CBigInt Add( CBigInt leftSide, CBigInt rightSide )
        {
            return leftSide - rightSide;
        }

        /// <summary>
        /// Decrements the BigInt operand by 1.
        /// </summary>
        /// <param name="leftSide">The BigInt operand.</param>
        /// <returns>The value of the <paramref name="leftSide" /> decremented by 1.</returns>
        public static CBigInt Decrement( CBigInt leftSide )
        {
            return ( leftSide - 1 );
        }

        /// <summary>
        /// Increments the BigInt operand by 1.
        /// </summary>
        /// <param name="leftSide">The BigInt operand.</param>
        /// <returns>The value of <paramref name="leftSide" /> incremented by 1.</returns>
        public static CBigInt Increment( CBigInt leftSide )
        {
            return ( leftSide + 1 );
        }

        /// <summary>
        /// Substracts two BigIntegers and returns a new BigInt that represents the sum.
        /// </summary>
        /// <param name="leftSide">A BigInt</param>
        /// <param name="rightSide">A BigInt</param>
        /// <returns>The BigInt result of substracting <paramref name="leftSide" /> and <paramref name="rightSide" />.</returns>
        public static CBigInt operator -( CBigInt leftSide, CBigInt rightSide )
        {
            int size = System.Math.Max ( leftSide.m_digits.DataUsed, rightSide.m_digits.DataUsed ) + 1;
            DigitsArray da = new DigitsArray ( size );

            long carry = 0;

            for ( int i = 0; i < da.Count; i++ )
            {
                long diff = ( long ) leftSide.m_digits[i] - ( long ) rightSide.m_digits[i] - carry;
                da[i] = (DType)( diff & DigitsArray.AllBits );
                da.DataUsed++;
                carry = ( ( diff < 0 ) ? 1 : 0 );
            }

            return new CBigInt( da );
        }

        /// <summary>
        /// Decrements the BigInt operand by 1.
        /// </summary>
        /// <param name="leftSide">The BigInt operand.</param>
        /// <returns>The value of the <paramref name="leftSide" /> decremented by 1.</returns>
        public static CBigInt operator --( CBigInt leftSide )
        {
            return ( leftSide - 1 );
        }

        /// <summary>
        /// Adds two BigIntegers and returns a new BigInt that represents the sum.
        /// </summary>
        /// <param name="leftSide">A BigInt</param>
        /// <param name="rightSide">A BigInt</param>
        /// <returns>The BigInt result of adding <paramref name="leftSide" /> and <paramref name="rightSide" />.</returns>
        public static CBigInt operator +( CBigInt leftSide, CBigInt rightSide )
        {
            int size = System.Math.Max ( leftSide.m_digits.DataUsed, rightSide.m_digits.DataUsed );
            DigitsArray da = new DigitsArray ( size + 1 );

            long carry = 0;

            for ( int i = 0; i < da.Count; i++ )
            {
                long sum = ( long ) leftSide.m_digits[i] + ( long ) rightSide.m_digits[i] + carry;
                carry = (long)( sum >> DigitsArray.DataSizeBits );
                da[i] = (DType)( sum & DigitsArray.AllBits );
            }

            return new CBigInt( da );
        }

        /// <summary>
        /// Increments the BigInt operand by 1.
        /// </summary>
        /// <param name="leftSide">The BigInt operand.</param>
        /// <returns>The value of <paramref name="leftSide" /> incremented by 1.</returns>
        public static CBigInt operator ++( CBigInt leftSide )
        {
            return ( leftSide + 1 );
        }

        /// <summary>
        /// Substracts two BigIntegers and returns a new BigInt that represents the sum.
        /// </summary>
        /// <param name="leftSide">A BigInt</param>
        /// <param name="rightSide">A BigInt</param>
        /// <returns>The BigInt result of substracting <paramref name="leftSide" /> and <paramref name="rightSide" />.</returns>
        public static CBigInt Subtract( CBigInt leftSide, CBigInt rightSide )
        {
            return leftSide - rightSide;
        }

        #endregion Addition and Subtraction Operator Overloads

        #region Negate Operator Overload

        /// <summary>
        /// Creates a BigInt absolute value of the operand.
        /// </summary>
        /// <param name="leftSide">A BigInt.</param>
        /// <returns>A BigInt that represents the absolute value of <paramref name="leftSide" />.</returns>
        public static CBigInt Abs( CBigInt leftSide )
        {
            if ( object.ReferenceEquals( leftSide, null ) )
            {
                throw new ArgumentNullException( "leftSide" );
            }

            if ( leftSide.IsNegative )
            {
                return -leftSide;
            }

            return leftSide;
        }

        /// <summary>
        /// Negates the BigInt, that is, if the BigInt is negative return a positive BigInt, and if the
        /// BigInt is negative return the postive.
        /// </summary>
        /// <param name="leftSide">A BigInt operand.</param>
        /// <returns>The value of the <paramref name="this" /> negated.</returns>
        public static CBigInt operator -( CBigInt leftSide )
        {
            if ( object.ReferenceEquals( leftSide, null ) )
            {
                throw new ArgumentNullException( "leftSide" );
            }

            if ( leftSide.IsZero )
            {
                return new CBigInt( 0 );
            }

            DigitsArray da = new DigitsArray ( leftSide.m_digits.DataUsed + 1, leftSide.m_digits.DataUsed + 1 );

            for ( int i = 0; i < da.Count; i++ )
            {
                da[i] = (DType)( ~( leftSide.m_digits[i] ) );
            }

            // add one to result (1's complement + 1)
            bool carry = true;
            int index = 0;

            while ( carry && index < da.Count )
            {
                long val = ( long ) da[index] + 1;
                da[index] = (DType)( val & DigitsArray.AllBits );
                carry = ( ( val >> DigitsArray.DataSizeBits ) > 0 );
                index++;
            }

            return new CBigInt( da );
        }

        /// <summary>
        /// Negates the BigInt, that is, if the BigInt is negative return a positive BigInt, and if the
        /// BigInt is negative return the postive.
        /// </summary>
        /// <returns>The value of the <paramref name="this" /> negated.</returns>
        public CBigInt Negate()
        {
            return -this;
        }

        #endregion Negate Operator Overload

        #region Multiplication, Division and Modulus Operators

        /// <summary>
        /// Divide a BigInt by another BigInt and returning the result.
        /// </summary>
        /// <param name="leftSide">A BigInt divisor.</param>
        /// <param name="rightSide">A BigInt dividend.</param>
        /// <returns>The BigInt result.</returns>
        public static CBigInt Divide( CBigInt leftSide, CBigInt rightSide )
        {
            return leftSide / rightSide;
        }

        public static void Divide( CBigInt leftSide, CBigInt rightSide, out CBigInt quotient, out CBigInt remainder )
        {
            if ( leftSide.IsZero )
            {
                quotient = new CBigInt();
                remainder = new CBigInt();
                return;
            }

            if ( rightSide.m_digits.DataUsed == 1 )
            {
                SingleDivide( leftSide, rightSide, out quotient, out remainder );
            }
            else
            {
                MultiDivide( leftSide, rightSide, out quotient, out remainder );
            }
        }

        /// <summary>
        /// Perform the modulus of a BigInt with another BigInt and return the result.
        /// </summary>
        /// <param name="leftSide">A BigInt divisor.</param>
        /// <param name="rightSide">A BigInt dividend.</param>
        /// <returns>The BigInt result.</returns>
        public static CBigInt Modulus( CBigInt leftSide, CBigInt rightSide )
        {
            return leftSide % rightSide;
        }

        /// <summary>
        /// Multiply two BigIntegers returning the result.
        /// </summary>
        /// <param name="leftSide">A BigInt.</param>
        /// <param name="rightSide">A BigInt</param>
        /// <returns></returns>
        public static CBigInt Multiply( CBigInt leftSide, CBigInt rightSide )
        {
            return leftSide * rightSide;
        }

        /// <summary>
        /// Perform the modulus of a BigInt with another BigInt and return the result.
        /// </summary>
        /// <param name="leftSide">A BigInt divisor.</param>
        /// <param name="rightSide">A BigInt dividend.</param>
        /// <returns>The BigInt result.</returns>
        public static CBigInt operator %( CBigInt leftSide, CBigInt rightSide )
        {
            if ( leftSide == null )
            {
                throw new ArgumentNullException( "leftSide" );
            }

            if ( rightSide == null )
            {
                throw new ArgumentNullException( "rightSide" );
            }

            if ( rightSide.IsZero )
            {
                throw new DivideByZeroException();
            }

            CBigInt quotient;
            CBigInt remainder;

            bool dividendNeg = leftSide.IsNegative;
            leftSide = Abs( leftSide );
            rightSide = Abs( rightSide );

            if ( leftSide < rightSide )
            {
                return leftSide;
            }

            Divide( leftSide, rightSide, out quotient, out remainder );

            return ( dividendNeg ? -remainder : remainder );
        }

        /// <summary>
        /// Multiply two BigIntegers returning the result.
        /// </summary>
        /// <remarks>
        /// See Knuth.
        /// </remarks>
        /// <param name="leftSide">A BigInt.</param>
        /// <param name="rightSide">A BigInt</param>
        /// <returns></returns>
        public static CBigInt operator *( CBigInt leftSide, CBigInt rightSide )
        {
            if ( object.ReferenceEquals( leftSide, null ) )
            {
                throw new ArgumentNullException( "leftSide" );
            }

            if ( object.ReferenceEquals( rightSide, null ) )
            {
                throw new ArgumentNullException( "rightSide" );
            }

            bool leftSideNeg = leftSide.IsNegative;
            bool rightSideNeg = rightSide.IsNegative;

            leftSide = Abs( leftSide );
            rightSide = Abs( rightSide );

            DigitsArray da = new DigitsArray ( leftSide.m_digits.DataUsed + rightSide.m_digits.DataUsed );
            da.DataUsed = da.Count;

            for ( int i = 0; i < leftSide.m_digits.DataUsed; i++ )
            {
                ulong carry = 0;

                for ( int j = 0, k = i; j < rightSide.m_digits.DataUsed; j++, k++ )
                {
                    ulong val = ( ( ulong ) leftSide.m_digits[i] * ( ulong ) rightSide.m_digits[j] ) + ( ulong ) da[k] + carry;

                    da[k] = (DType)( val & DigitsArray.AllBits );
                    carry = ( val >> DigitsArray.DataSizeBits );
                }

                if ( carry != 0 )
                {
                    da[i + rightSide.m_digits.DataUsed] = (DType)carry;
                }
            }

            //da.ResetDataUsed();
            CBigInt result = new CBigInt ( da );
            return ( leftSideNeg != rightSideNeg ? -result : result );
        }

        /// <summary>
        /// Divide a BigInt by another BigInt and returning the result.
        /// </summary>
        /// <param name="leftSide">A BigInt divisor.</param>
        /// <param name="rightSide">A BigInt dividend.</param>
        /// <returns>The BigInt result.</returns>
        public static CBigInt operator /( CBigInt leftSide, CBigInt rightSide )
        {
            if ( leftSide == null )
            {
                throw new ArgumentNullException( "leftSide" );
            }

            if ( rightSide == null )
            {
                throw new ArgumentNullException( "rightSide" );
            }

            if ( rightSide.IsZero )
            {
                throw new DivideByZeroException();
            }

            bool divisorNeg = rightSide.IsNegative;
            bool dividendNeg = leftSide.IsNegative;

            leftSide = Abs( leftSide );
            rightSide = Abs( rightSide );

            if ( leftSide < rightSide )
            {
                return new CBigInt( 0 );
            }

            CBigInt quotient;
            CBigInt remainder;
            Divide( leftSide, rightSide, out quotient, out remainder );

            return ( dividendNeg != divisorNeg ? -quotient : quotient );
        }

        private static void MultiDivide( CBigInt leftSide, CBigInt rightSide, out CBigInt quotient, out CBigInt remainder )
        {
            if ( rightSide.IsZero )
            {
                throw new DivideByZeroException();
            }

            DType val = rightSide.m_digits[rightSide.m_digits.DataUsed - 1];
            int d = 0;

            for ( uint mask = DigitsArray.HiBitSet; mask != 0 && ( val & mask ) == 0; mask >>= 1 )
            {
                d++;
            }

            int remainderLen = leftSide.m_digits.DataUsed + 1;
            DType[] remainderDat = new DType[remainderLen];
            leftSide.m_digits.CopyTo( remainderDat, 0, leftSide.m_digits.DataUsed );

            DigitsArray.ShiftLeft( remainderDat, d );
            rightSide = rightSide << d;

            ulong firstDivisor = rightSide.m_digits[rightSide.m_digits.DataUsed - 1];
            ulong secondDivisor = ( rightSide.m_digits.DataUsed < 2 ? ( DType ) 0 : rightSide.m_digits[rightSide.m_digits.DataUsed - 2] );

            int divisorLen = rightSide.m_digits.DataUsed + 1;
            DigitsArray dividendPart = new DigitsArray ( divisorLen, divisorLen );
            DType[] result = new DType[leftSide.m_digits.Count + 1];
            int resultPos = 0;

            ulong carryBit = ( ulong ) 0x1 << DigitsArray.DataSizeBits; // 0x100000000

            for ( int j = remainderLen - rightSide.m_digits.DataUsed, pos = remainderLen - 1; j > 0; j--, pos-- )
            {
                ulong dividend = ( ( ulong ) remainderDat[pos] << DigitsArray.DataSizeBits ) + ( ulong ) remainderDat[pos - 1];
                ulong qHat = ( dividend / firstDivisor );
                ulong rHat = ( dividend % firstDivisor );

                while ( pos >= 2 )
                {
                    if ( qHat == carryBit || ( qHat * secondDivisor ) > ( ( rHat << DigitsArray.DataSizeBits ) + remainderDat[pos - 2] ) )
                    {
                        qHat--;
                        rHat += firstDivisor;

                        if ( rHat < carryBit )
                        {
                            continue;
                        }
                    }

                    break;
                }

                for ( int h = 0; h < divisorLen; h++ )
                {
                    dividendPart[divisorLen - h - 1] = remainderDat[pos - h];
                }

                CBigInt dTemp = new CBigInt ( dividendPart );
                CBigInt rTemp = rightSide * ( long ) qHat;

                while ( rTemp > dTemp )
                {
                    qHat--;
                    rTemp -= rightSide;
                }

                rTemp = dTemp - rTemp;

                for ( int h = 0; h < divisorLen; h++ )
                {
                    remainderDat[pos - h] = rTemp.m_digits[rightSide.m_digits.DataUsed - h];
                }

                result[resultPos++] = (DType)qHat;
            }

            Array.Reverse( result, 0, resultPos );
            quotient = new CBigInt( new DigitsArray( result ) );

            int n = DigitsArray.ShiftRight ( remainderDat, d );
            DigitsArray rDA = new DigitsArray ( n, n );
            rDA.CopyFrom( remainderDat, 0, 0, rDA.DataUsed );
            remainder = new CBigInt( rDA );
        }

        private static void SingleDivide( CBigInt leftSide, CBigInt rightSide, out CBigInt quotient, out CBigInt remainder )
        {
            if ( rightSide.IsZero )
            {
                throw new DivideByZeroException();
            }

            DigitsArray remainderDigits = new DigitsArray ( leftSide.m_digits );
            remainderDigits.ResetDataUsed();

            int pos = remainderDigits.DataUsed - 1;
            ulong divisor = ( ulong ) rightSide.m_digits[0];
            ulong dividend = ( ulong ) remainderDigits[pos];

            DType[] result = new DType[leftSide.m_digits.Count];
            leftSide.m_digits.CopyTo( result, 0, result.Length );
            int resultPos = 0;

            if ( dividend >= divisor )
            {
                result[resultPos++] = (DType)( dividend / divisor );
                remainderDigits[pos] = (DType)( dividend % divisor );
            }

            pos--;

            while ( pos >= 0 )
            {
                dividend = ( (ulong)( remainderDigits[pos + 1] ) << DigitsArray.DataSizeBits ) + (ulong)remainderDigits[pos];
                result[resultPos++] = (DType)( dividend / divisor );
                remainderDigits[pos + 1] = 0;
                remainderDigits[pos--] = (DType)( dividend % divisor );
            }

            remainder = new CBigInt( remainderDigits );

            DigitsArray quotientDigits = new DigitsArray ( resultPos + 1, resultPos );
            int j = 0;

            for ( int i = quotientDigits.DataUsed - 1; i >= 0; i--, j++ )
            {
                quotientDigits[j] = result[i];
            }

            quotient = new CBigInt( quotientDigits );
        }

        #endregion Multiplication, Division and Modulus Operators

        #region Bitwise Operator Overloads

        public static CBigInt BitwiseAnd( CBigInt leftSide, CBigInt rightSide )
        {
            return leftSide & rightSide;
        }

        public static CBigInt BitwiseOr( CBigInt leftSide, CBigInt rightSide )
        {
            return leftSide | rightSide;
        }

        public static CBigInt OnesComplement( CBigInt leftSide )
        {
            return ~leftSide;
        }

        public static CBigInt operator &( CBigInt leftSide, CBigInt rightSide )
        {
            int len = System.Math.Max ( leftSide.m_digits.DataUsed, rightSide.m_digits.DataUsed );
            DigitsArray da = new DigitsArray ( len, len );

            for ( int idx = 0; idx < len; idx++ )
            {
                da[idx] = (DType)( leftSide.m_digits[idx] & rightSide.m_digits[idx] );
            }

            return new CBigInt( da );
        }

        public static CBigInt operator ^( CBigInt leftSide, CBigInt rightSide )
        {
            int len = System.Math.Max ( leftSide.m_digits.DataUsed, rightSide.m_digits.DataUsed );
            DigitsArray da = new DigitsArray ( len, len );

            for ( int idx = 0; idx < len; idx++ )
            {
                da[idx] = (DType)( leftSide.m_digits[idx] ^ rightSide.m_digits[idx] );
            }

            return new CBigInt( da );
        }

        public static CBigInt operator |( CBigInt leftSide, CBigInt rightSide )
        {
            int len = System.Math.Max ( leftSide.m_digits.DataUsed, rightSide.m_digits.DataUsed );
            DigitsArray da = new DigitsArray ( len, len );

            for ( int idx = 0; idx < len; idx++ )
            {
                da[idx] = (DType)( leftSide.m_digits[idx] | rightSide.m_digits[idx] );
            }

            return new CBigInt( da );
        }

        public static CBigInt operator ~( CBigInt leftSide )
        {
            DigitsArray da = new DigitsArray ( leftSide.m_digits.Count );

            for ( int idx = 0; idx < da.Count; idx++ )
            {
                da[idx] = (DType)( ~( leftSide.m_digits[idx] ) );
            }

            return new CBigInt( da );
        }

        public static CBigInt Xor( CBigInt leftSide, CBigInt rightSide )
        {
            return leftSide ^ rightSide;
        }

        #endregion Bitwise Operator Overloads

        #region Left and Right Shift Operator Overloads

        public static CBigInt LeftShift( CBigInt leftSide, int shiftCount )
        {
            return leftSide << shiftCount;
        }

        public static CBigInt operator <<( CBigInt leftSide, int shiftCount )
        {
            if ( leftSide == null )
            {
                throw new ArgumentNullException( "leftSide" );
            }

            DigitsArray da = new DigitsArray ( leftSide.m_digits );
            da.DataUsed = da.ShiftLeftWithoutOverflow( shiftCount );

            return new CBigInt( da );
        }

        public static CBigInt operator >>( CBigInt leftSide, int shiftCount )
        {
            if ( leftSide == null )
            {
                throw new ArgumentNullException( "leftSide" );
            }

            DigitsArray da = new DigitsArray ( leftSide.m_digits );
            da.DataUsed = da.ShiftRight( shiftCount );

            if ( leftSide.IsNegative )
            {
                for ( int i = da.Count - 1; i >= da.DataUsed; i-- )
                {
                    da[i] = DigitsArray.AllBits;
                }

                DType mask = DigitsArray.HiBitSet;

                for ( int i = 0; i < DigitsArray.DataSizeBits; i++ )
                {
                    if ( ( da[da.DataUsed - 1] & mask ) == DigitsArray.HiBitSet )
                    {
                        break;
                    }

                    da[da.DataUsed - 1] |= mask;
                    mask >>= 1;
                }

                da.DataUsed = da.Count;
            }

            return new CBigInt( da );
        }

        public static CBigInt RightShift( CBigInt leftSide, int shiftCount )
        {
            if ( leftSide == null )
            {
                throw new ArgumentNullException( "leftSide" );
            }

            return leftSide >> shiftCount;
        }

        #endregion Left and Right Shift Operator Overloads

        #region Relational Operator Overloads

        /// <summary>
        /// Compare two objects and return an indication of their relative value.
        /// </summary>
        /// <param name="leftSide">An object to compare, or a null reference (<b>Nothing</b> in Visual Basic).</param>
        /// <param name="rightSide">An object to compare, or a null reference (<b>Nothing</b> in Visual Basic).</param>
        /// <returns>A signed number indicating the relative value of this instance and <i>value</i>.
        /// <list type="table">
        ///        <listheader>
        ///            <term>Return Value</term>
        ///            <description>Description</description>
        ///        </listheader>
        ///        <item>
        ///            <term>Less than zero</term>
        ///            <description>This instance is less than <i>value</i>.</description>
        ///        </item>
        ///        <item>
        ///            <term>Zero</term>
        ///            <description>This instance is equal to <i>value</i>.</description>
        ///        </item>
        ///        <item>
        ///            <term>Greater than zero</term>
        ///            <description>
        ///                This instance is greater than <i>value</i>.
        ///                <para>-or-</para>
        ///                <i>value</i> is a null reference (<b>Nothing</b> in Visual Basic).
        ///            </description>
        ///        </item>
        /// </list>
        /// </returns>
        public static int Compare( CBigInt leftSide, CBigInt rightSide )
        {
            if ( object.ReferenceEquals( leftSide, rightSide ) )
            {
                return 0;
            }

            if ( object.ReferenceEquals( leftSide, null ) )
            {
                throw new ArgumentNullException( "leftSide" );
            }

            if ( object.ReferenceEquals( rightSide, null ) )
            {
                throw new ArgumentNullException( "rightSide" );
            }

            if ( leftSide > rightSide )
            {
                return 1;
            }

            if ( leftSide == rightSide )
            {
                return 0;
            }

            return -1;
        }

        public static bool operator !=( CBigInt leftSide, CBigInt rightSide )
        {
            return !( leftSide == rightSide );
        }

        public static bool operator <( CBigInt leftSide, CBigInt rightSide )
        {
            if ( object.ReferenceEquals( leftSide, null ) )
            {
                throw new ArgumentNullException( "leftSide" );
            }

            if ( object.ReferenceEquals( rightSide, null ) )
            {
                throw new ArgumentNullException( "rightSide" );
            }

            if ( leftSide.IsNegative != rightSide.IsNegative )
            {
                return leftSide.IsNegative;
            }

            if ( leftSide.m_digits.DataUsed != rightSide.m_digits.DataUsed )
            {
                return leftSide.m_digits.DataUsed < rightSide.m_digits.DataUsed;
            }

            for ( int idx = leftSide.m_digits.DataUsed - 1; idx >= 0; idx-- )
            {
                if ( leftSide.m_digits[idx] != rightSide.m_digits[idx] )
                {
                    return ( leftSide.m_digits[idx] < rightSide.m_digits[idx] );
                }
            }

            return false;
        }

        public static bool operator <=( CBigInt leftSide, CBigInt rightSide )
        {
            return Compare( leftSide, rightSide ) <= 0;
        }

        public static bool operator ==( CBigInt leftSide, CBigInt rightSide )
        {
            if ( object.ReferenceEquals( leftSide, rightSide ) )
            {
                return true;
            }

            if ( object.ReferenceEquals( leftSide, null ) || object.ReferenceEquals( rightSide, null ) )
            {
                return false;
            }

            if ( leftSide.IsNegative != rightSide.IsNegative )
            {
                return false;
            }

            return leftSide.Equals( rightSide );
        }

        public static bool operator >( CBigInt leftSide, CBigInt rightSide )
        {
            if ( object.ReferenceEquals( leftSide, null ) )
            {
                throw new ArgumentNullException( "leftSide" );
            }

            if ( object.ReferenceEquals( rightSide, null ) )
            {
                throw new ArgumentNullException( "rightSide" );
            }

            if ( leftSide.IsNegative != rightSide.IsNegative )
            {
                return rightSide.IsNegative;
            }

            if ( leftSide.m_digits.DataUsed != rightSide.m_digits.DataUsed )
            {
                return leftSide.m_digits.DataUsed > rightSide.m_digits.DataUsed;
            }

            for ( int idx = leftSide.m_digits.DataUsed - 1; idx >= 0; idx-- )
            {
                if ( leftSide.m_digits[idx] != rightSide.m_digits[idx] )
                {
                    return ( leftSide.m_digits[idx] > rightSide.m_digits[idx] );
                }
            }

            return false;
        }

        public static bool operator >=( CBigInt leftSide, CBigInt rightSide )
        {
            return Compare( leftSide, rightSide ) >= 0;
        }

        /// <summary>
        /// Compare this instance to a specified object and returns indication of their relative value.
        /// </summary>
        /// <param name="value">An object to compare, or a null reference (<b>Nothing</b> in Visual Basic).</param>
        /// <returns>A signed number indicating the relative value of this instance and <i>value</i>.
        /// <list type="table">
        ///        <listheader>
        ///            <term>Return Value</term>
        ///            <description>Description</description>
        ///        </listheader>
        ///        <item>
        ///            <term>Less than zero</term>
        ///            <description>This instance is less than <i>value</i>.</description>
        ///        </item>
        ///        <item>
        ///            <term>Zero</term>
        ///            <description>This instance is equal to <i>value</i>.</description>
        ///        </item>
        ///        <item>
        ///            <term>Greater than zero</term>
        ///            <description>
        ///                This instance is greater than <i>value</i>.
        ///                <para>-or-</para>
        ///                <i>value</i> is a null reference (<b>Nothing</b> in Visual Basic).
        ///            </description>
        ///        </item>
        /// </list>
        /// </returns>
        public int CompareTo( CBigInt value )
        {
            return Compare( this, value );
        }

        #endregion Relational Operator Overloads

        #region Object Overrides

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">An <see cref="System.Object">Object</see> to compare with this instance.</param>
        /// <returns></returns>
        /// <seealso cref="System.Object">System.Object</seealso>
        public override bool Equals( object obj )
        {
            if ( object.ReferenceEquals( obj, null ) )
            {
                return false;
            }

            if ( object.ReferenceEquals( this, obj ) )
            {
                return true;
            }

            CBigInt c = ( CBigInt ) obj;

            if ( this.m_digits.DataUsed != c.m_digits.DataUsed )
            {
                return false;
            }

            for ( int idx = 0; idx < this.m_digits.DataUsed; idx++ )
            {
                if ( this.m_digits[idx] != c.m_digits[idx] )
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer has code.</returns>
        /// <seealso cref="System.Object">System.Object</seealso>
        public override int GetHashCode()
        {
            return this.m_digits.GetHashCode();
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent base 10 string representation.
        /// </summary>
        /// <returns>A <see cref="System.String">String</see> in base 10.</returns>
        /// <seealso cref="System.Object">System.Object</seealso>
        public override string ToString()
        {
            return ToString( 10 );
        }

        #endregion Object Overrides

        #region Type Conversion Methods

        /// <summary>
        /// Returns BigInt as System.Int16 if possible.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Int value of BigInt</returns>
        /// <exception cref="System.Exception">When BigInt is too large to fit into System.Int16</exception>
        public static int ToInt16( CBigInt value )
        {
            if ( object.ReferenceEquals( value, null ) )
            {
                throw new ArgumentNullException( "value" );
            }

            return System.Int16.Parse( value.ToString(), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns BigInt as System.Int32 if possible.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception">When BigInt is too large to fit into System.Int32</exception>
        public static int ToInt32( CBigInt value )
        {
            if ( object.ReferenceEquals( value, null ) )
            {
                throw new ArgumentNullException( "value" );
            }

            return System.Int32.Parse( value.ToString(), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns BigInt as System.Int64 if possible.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception">When BigInt is too large to fit into System.Int64</exception>
        public static long ToInt64( CBigInt value )
        {
            if ( object.ReferenceEquals( value, null ) )
            {
                throw new ArgumentNullException( "value" );
            }

            return System.Int64.Parse( value.ToString(), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns BigInt as System.UInt16 if possible.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception">When BigInt is too large to fit into System.UInt16</exception>
        public static uint ToUInt16( CBigInt value )
        {
            if ( object.ReferenceEquals( value, null ) )
            {
                throw new ArgumentNullException( "value" );
            }

            return System.UInt16.Parse( value.ToString(), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns BigInt as System.UInt32 if possible.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception">When BigInt is too large to fit into System.UInt32</exception>
        public static uint ToUInt32( CBigInt value )
        {
            if ( object.ReferenceEquals( value, null ) )
            {
                throw new ArgumentNullException( "value" );
            }

            return System.UInt32.Parse( value.ToString(), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns BigInt as System.UInt64 if possible.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception">When BigInt is too large to fit into System.UInt64</exception>
        public static ulong ToUInt64( CBigInt value )
        {
            if ( object.ReferenceEquals( value, null ) )
            {
                throw new ArgumentNullException( "value" );
            }

            return System.UInt64.Parse( value.ToString(), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.CurrentCulture );
        }

        /// <summary>
        /// Returns string in hexidecimal of the internal digit representation.
        /// </summary>
        /// <remarks>
        /// This is not the same as ToString(16). This method does not return the sign, but instead
        /// dumps the digits array into a string representation in base 16.
        /// </remarks>
        /// <returns>A string in base 16.</returns>
        public string ToHexString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat( "{0:X}", m_digits[m_digits.DataUsed - 1] );

            string f = "{0:X" + ( 2 * DigitsArray.DataSizeOf ) + "}";

            for ( int i = m_digits.DataUsed - 2; i >= 0; i-- )
            {
                sb.AppendFormat( f, m_digits[i] );
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation in specified base.
        /// </summary>
        /// <param name="radix">Int radix between 2 and 36</param>
        /// <returns>A string.</returns>
        public string ToString( int radix )
        {
            if ( radix < 2 || radix > 36 )
            {
                throw new ArgumentOutOfRangeException( "radix" );
            }

            if ( IsZero )
            {
                return "0";
            }

            CBigInt a = this;
            bool negative = a.IsNegative;
            a = Abs( this );

            CBigInt quotient;
            CBigInt remainder;
            CBigInt biRadix = new CBigInt ( radix );

            const string charSet = "0123456789abcdefghijklmnopqrstuvwxyz";
            System.Collections.ArrayList al = new System.Collections.ArrayList();

            while ( a.m_digits.DataUsed > 1 || ( a.m_digits.DataUsed == 1 && a.m_digits[0] != 0 ) )
            {
                Divide( a, biRadix, out quotient, out remainder );
                al.Insert( 0, charSet[(int)remainder.m_digits[0]] );
                a = quotient;
            }

            string result = new String ( ( char[] ) al.ToArray ( typeof ( char ) ) );

            if ( radix == 10 && negative )
            {
                return "-" + result;
            }

            return result;
        }

        #endregion Type Conversion Methods
    }
}