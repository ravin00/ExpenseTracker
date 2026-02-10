import { useState } from 'react'

interface DateRangePickerProps {
    onRangeChange: (startDate: string, endDate: string) => void
    initialStartDate?: string
    initialEndDate?: string
}

export function DateRangePicker({ 
    onRangeChange, 
    initialStartDate = '', 
    initialEndDate = '' 
}: DateRangePickerProps) {
    const [startDate, setStartDate] = useState(initialStartDate)
    const [endDate, setEndDate] = useState(initialEndDate)

    const handleApply = () => {
        if (startDate && endDate) {
            onRangeChange(startDate, endDate)
        }
    }

    const handleQuickSelect = (days: number) => {
        const end = new Date()
        const start = new Date()
        start.setDate(end.getDate() - days)
        
        const startStr = start.toISOString().split('T')[0]
        const endStr = end.toISOString().split('T')[0]
        
        setStartDate(startStr)
        setEndDate(endStr)
        onRangeChange(startStr, endStr)
    }

    return (
        <div className="date-range-picker">
            <div className="quick-select">
                <button onClick={() => handleQuickSelect(7)}>Last 7 Days</button>
                <button onClick={() => handleQuickSelect(30)}>Last 30 Days</button>
                <button onClick={() => handleQuickSelect(90)}>Last 90 Days</button>
            </div>
            <div className="custom-range">
                <input 
                    type="date" 
                    value={startDate}
                    onChange={(e) => setStartDate(e.target.value)}
                    placeholder="Start Date"
                />
                <span>to</span>
                <input 
                    type="date" 
                    value={endDate}
                    onChange={(e) => setEndDate(e.target.value)}
                    placeholder="End Date"
                />
                <button onClick={handleApply}>Apply</button>
            </div>
        </div>
    )
}